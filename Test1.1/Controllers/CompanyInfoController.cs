using Microsoft.AspNetCore.Mvc;
using Test1._1.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using Test1._1.Models.Entity;
using Test1._1.Models.ViewModels;
using System;

namespace Test1._1.Controllers
{
	public class CompanyInfoController : Controller
	{
		private readonly AppDBContext _context;
		private readonly IWebHostEnvironment _env;

		public CompanyInfoController(AppDBContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}

        public async Task<IActionResult> Index(string id)
		{
			if (string.IsNullOrEmpty(id))
				return RedirectToAction("ErrorPage");

            var company = await _context.Companies
                .Include(c => c.JobAdvertisments)
                .FirstOrDefaultAsync(c => c.Id == id);

			if (company == null)
				return NotFound();

			// جلب الاشتراكات
			var subs = _context.CompanySubscraptions.ToList();
			ViewBag.CompanySubscraptions = subs;

            var recentJobTitles = company.JobAdvertisments
                .OrderByDescending(j => j.CreatedDate)
                .Take(5)
                .Select(j => j.jobtitle)
                .ToList();
			// جلب الاشتراك المفعّل الحالي
			var activeTransaction = _context.CompanyTransactions
				.Include(t => t.CompanySubscraption)
				.FirstOrDefault(t => t.CompanyId == company.Id && t.IsPaid && t.IsActive);

			ViewBag.ActiveTransaction = activeTransaction;
            var allApplicants = _context.Applicants.ToList();

            var suggestedApplicants = allApplicants
                .Select(app => new
                {
                    Applicant = app,
                    Score =
                        recentJobTitles.Sum(title =>
                            (title.Equals(app.Field_work, StringComparison.OrdinalIgnoreCase) ? 50 : 0) +
                            (title.Contains(app.Field_work, StringComparison.OrdinalIgnoreCase) ? 20 : 0)
                        )
                        + app.Years_experience
                        + (app.address == company.address ? 10 : 0)
                })
                .OrderByDescending(a => a.Score)
                .Take(8)
                .Select(a => new ApplicantCardHomeViewModel
                {
                    Id = a.Applicant.Id,
                    Name = a.Applicant.UserName,
                    LastName = a.Applicant.lastName,
                    FieldWork = a.Applicant.Field_work,
                    ImagePath = a.Applicant.Profile_image
                })
                .ToList();

            var viewModel = new CompanyProfileViewModel
            {
                Company = company,
                JobAdvertisments = company.JobAdvertisments.ToList(),
                SuggestedApplicants = suggestedApplicants
            };

            return View(viewModel);
		}


		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompanyEditViewModel model)
        {
            // Clear all ModelState errors since we're doing custom validation
            ModelState.Clear();

            // Use custom validation
            if (!model.IsValidForUpdate())
            {
                var errors = model.GetValidationErrors();
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                // Reload the company data for the view
                var companyForError = await _context.Companies.FirstOrDefaultAsync(c => c.Id == model.Id);
                if (companyForError != null)
                {
                    model.OriginalUserName = companyForError.UserName;
                    model.OriginalPhoneNumber = companyForError.PhoneNumber;
                    model.OriginalEmail = companyForError.Email;
                    model.OriginalFiledWork = companyForError.FiledWork;
                    model.OriginalAddress = companyForError.address;
                    model.OriginalDescription = companyForError.Description;
                    model.CurrentLogo = companyForError.Logo;
                    model.CurrentTaxCard = companyForError.TaxCard;
                    model.CurrentCommercialRegister = companyForError.CommercialRegister;
                }

                return View(model);
            }

            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == model.Id);
            if (company == null)
                return NotFound();

            try
            {
                bool hasChanges = false;

                // Update fields only if they are provided and different
                if (!string.IsNullOrWhiteSpace(model.UserName) && model.UserName.Trim() != company.UserName?.Trim())
                {
                    company.UserName = model.UserName.Trim();
                    hasChanges = true;
                }

                if (!string.IsNullOrWhiteSpace(model.Phone) && model.Phone.Trim() != company.PhoneNumber?.Trim())
                {
                    company.PhoneNumber = model.Phone.Trim();
                    hasChanges = true;
                }

                if (!string.IsNullOrWhiteSpace(model.Email) && model.Email.Trim() != company.Email?.Trim())
                {
                    company.Email = model.Email.Trim();
                    hasChanges = true;
                }

                if (!string.IsNullOrWhiteSpace(model.FiledWork) && model.FiledWork.Trim() != company.FiledWork?.Trim())
                {
                    company.FiledWork = model.FiledWork.Trim();
                    hasChanges = true;
                }

                if (!string.IsNullOrWhiteSpace(model.Address) && model.Address.Trim() != company.address?.Trim())
                {
                    company.address = model.Address.Trim();
                    hasChanges = true;
                }

                if (!string.IsNullOrWhiteSpace(model.Description) && model.Description.Trim() != company.Description?.Trim())
                {
                    company.Description = model.Description.Trim();
                    hasChanges = true;
                }

                // Handle password change only if provided
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    // TODO: Hash the password properly in production
                    company.PasswordHash = model.Password;
                    hasChanges = true;
                }

                // Handle file uploads
                if (model.LogoFile != null && model.LogoFile.Length > 0)
                {
                    var result = await HandleFileUpload(model.LogoFile, "logos", new[] { ".jpg", ".jpeg", ".png" });
                    if (result.Success)
                    {
                        // Delete old logo if exists
                        if (!string.IsNullOrEmpty(company.Logo))
                        {
                            DeleteOldFile(company.Logo);
                        }
                        company.Logo = result.FilePath;
                        hasChanges = true;
                    }
                    else
                    {
                        ModelState.AddModelError("LogoFile", result.ErrorMessage);
                        return View(model);
                    }
                }

                if (model.TaxCardFile != null && model.TaxCardFile.Length > 0)
                {
                    var result = await HandleFileUpload(model.TaxCardFile, "documents", new[] { ".pdf" });
                    if (result.Success)
                    {
                        // Delete old tax card if exists
                        if (!string.IsNullOrEmpty(company.TaxCard))
                        {
                            DeleteOldFile(company.TaxCard);
                        }
                        company.TaxCard = result.FilePath;
                        hasChanges = true;
                    }
                    else
                    {
                        ModelState.AddModelError("TaxCardFile", result.ErrorMessage);
                        return View(model);
                    }
                }

                if (model.CommercialRegisterFile != null && model.CommercialRegisterFile.Length > 0)
                {
                    var result = await HandleFileUpload(model.CommercialRegisterFile, "documents", new[] { ".pdf" });
                    if (result.Success)
                    {
                        // Delete old commercial register if exists
                        if (!string.IsNullOrEmpty(company.CommercialRegister))
                        {
                            DeleteOldFile(company.CommercialRegister);
                        }
                        company.CommercialRegister = result.FilePath;
                        hasChanges = true;
                    }
                    else
		{
                        ModelState.AddModelError("CommercialRegisterFile", result.ErrorMessage);
                        return View(model);
                    }
                }

                // Save changes if any were made
                if (hasChanges)
                {
                    await _context.SaveChangesAsync();
                }

                // Always redirect to Index after successful processing
                return RedirectToAction("Index", new { id = company.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while saving changes: {ex.Message}");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Edit(string id)
			{
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var company = _context.Companies.FirstOrDefault(c => c.Id == id);
            if (company == null)
                return NotFound();

            var model = new CompanyEditViewModel
            {
                Id = company.Id,
                UserName = company.UserName,
                Phone = company.PhoneNumber,
                Email = company.Email,
                FiledWork = company.FiledWork,
                Address = company.address,
                Description = company.Description,
                CurrentLogo = company.Logo,
                CurrentTaxCard = company.TaxCard,
                CurrentCommercialRegister = company.CommercialRegister,

                // Store original values
                OriginalUserName = company.UserName,
                OriginalPhoneNumber = company.PhoneNumber,
                OriginalEmail = company.Email,
                OriginalFiledWork = company.FiledWork,
                OriginalAddress = company.address,
                OriginalDescription = company.Description
            };

            return View(model);
		}

        private async Task<(bool Success, string FilePath, string ErrorMessage)> HandleFileUpload(
            IFormFile file, string folder, string[] allowedExtensions)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
		{
                return (false, null, $"Invalid file type. Allowed types: {string.Join(", ", allowedExtensions)}");
            }

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var folderPath = Path.Combine(_env.WebRootPath, folder);
            var savePath = Path.Combine(folderPath, fileName);

            Directory.CreateDirectory(folderPath);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return (true, $"{folder}/{fileName}", null);
        }

        private void DeleteOldFile(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return;

            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
		}

		[HttpGet]
		public IActionResult DownloadTaxCard(string id)
		{
			if (string.IsNullOrEmpty(id))
				return NotFound();

			var company = _context.Companies.FirstOrDefault(c => c.Id == id);
			if (company == null || string.IsNullOrEmpty(company.TaxCard))
				return NotFound("Company or Tax Card not found.");

			return DownloadFile(company.TaxCard, "TaxCard");
		}

		[HttpGet]
		public IActionResult DownloadCommercialRegister(string id)
		{
			if (string.IsNullOrEmpty(id))
				return NotFound();

			var company = _context.Companies.FirstOrDefault(c => c.Id == id);
			if (company == null || string.IsNullOrEmpty(company.CommercialRegister))
				return NotFound("Company or Commercial Register not found.");

			return DownloadFile(company.CommercialRegister, "CommercialRegister");
		}

		private IActionResult DownloadFile(string relativePath, string fileType)
		{
			var normalizedPath = relativePath.StartsWith("/") ? relativePath.Substring(1) : relativePath;
			var fullPath = Path.Combine(_env.WebRootPath, normalizedPath);

			if (!System.IO.File.Exists(fullPath))
				return NotFound($"{fileType} file not found on the server.");

			var fileBytes = System.IO.File.ReadAllBytes(fullPath);
			var contentType = "application/pdf";
			var fileName = Path.GetFileName(fullPath);

			return File(fileBytes, contentType, fileName);
		}

        public IActionResult ListCompanies()
        {
            var companies = _context.Companies
                .Select(c => new { c.Id, c.UserName, c.Email })
                .ToList();

            return Json(companies);
        }


        [HttpPost]
        public IActionResult Subscribe(int subId, string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
                return RedirectToAction("Index", "Home");

            var subscription = _context.CompanySubscraptions.FirstOrDefault(s => s.Id == subId);
            if (subscription == null)
                return RedirectToAction("Index", new { id = companyId });

            string refCode = "JOB-" + new Random().Next(1000, 9999);

            var transaction = new CompanyTransaction
            {
                CompanyId = companyId,
                CompanySubscraptionId = subId,
                Amount = subscription.Price,
                ReferenceCode = refCode,
                PaymentDate = DateTime.Now,
                IsPaid = false,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                IsActive = false
            };

            _context.CompanyTransactions.Add(transaction);
            _context.SaveChanges();

            TempData["RefCode"] = refCode;
            TempData["Amount"] = subscription.Price.ToString();
            TempData["CompanyId"] = companyId;
            TempData.Keep("CompanyId");

            return RedirectToAction("PaymentInstructions");
        }

        public IActionResult PaymentInstructions()
        {
            var refCode = TempData["RefCode"]?.ToString();
            var amountStr = TempData["Amount"]?.ToString();
            var companyId = TempData["CompanyId"]?.ToString();

            if (string.IsNullOrEmpty(refCode) || string.IsNullOrEmpty(amountStr))
                return RedirectToAction("Index", "Home");

            if (!decimal.TryParse(amountStr, out decimal amount))
                return RedirectToAction("Index", "Home");

            ViewBag.RefCode = refCode;
            ViewBag.Amount = amount;
            ViewBag.CompanyId = companyId;

            return View();
        }

    }
}
