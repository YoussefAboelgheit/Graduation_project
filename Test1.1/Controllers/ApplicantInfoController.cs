using Microsoft.AspNetCore.Mvc;
using Test1._1.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using Test1._1.Models.ViewModels;

namespace Test1._1.Controllers
{
    public class ApplicantInfoController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _env;

        public ApplicantInfoController(AppDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("ErrorPage");

            var applicant = _context.Applicants
                .Include(a => a.ApplicantAdvertisments)
                .ThenInclude(aa => aa.JobAdvertisment)
                .ThenInclude(ja => ja.Company)
                .FirstOrDefault(a => a.Id == id);

            if (applicant == null)
                return NotFound();

            return View(applicant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicantEditViewModel model)
        {
            // Clear all ModelState errors since we're doing custom validation
            ModelState.Clear();

            // Handle "Other" field work selection
            string fieldWorkOther = Request.Form["Field_work_other"];
            if (!string.IsNullOrWhiteSpace(fieldWorkOther))
            {
                model.Field_work = fieldWorkOther.Trim();
            }

            // Use custom validation
            if (!model.IsValidForUpdate())
            {
                var errors = model.GetValidationErrors();
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }

                // Reload the applicant data for the view
                var applicantForError = await _context.Applicants.FirstOrDefaultAsync(a => a.Id == model.Id);
                if (applicantForError != null)
                {
                    model.OriginalUserName = applicantForError.UserName;
                    model.OriginalLastName = applicantForError.lastName;
                    model.OriginalPhoneNumber = applicantForError.PhoneNumber;
                    model.OriginalEmail = applicantForError.Email;
                    model.OriginalField_work = applicantForError.Field_work;
                    model.OriginalYears_experience = applicantForError.Years_experience;
                    model.OriginalAddress = applicantForError.address;
                    model.CurrentCV = applicantForError.CV;
                    model.CurrentProfileImage = applicantForError.Profile_image;
                }

                return View(model);
            }

            var applicant = await _context.Applicants.FirstOrDefaultAsync(a => a.Id == model.Id);
            if (applicant == null)
                return NotFound();

            try
            {
                bool hasChanges = false;

                // Update fields only if they are provided and different
                if (!string.IsNullOrWhiteSpace(model.UserName) && model.UserName.Trim() != applicant.UserName?.Trim())
                {
                    applicant.UserName = model.UserName.Trim();
                    hasChanges = true;
                }

                if (!string.IsNullOrWhiteSpace(model.lastName) && model.lastName.Trim() != applicant.lastName?.Trim())
                {
                    applicant.lastName = model.lastName.Trim();
                    hasChanges = true;
                }

                if (!string.IsNullOrWhiteSpace(model.PhoneNumber) && model.PhoneNumber.Trim() != applicant.PhoneNumber?.Trim())
                {
                    applicant.PhoneNumber = model.PhoneNumber.Trim();
                    hasChanges = true;
                }

                if (!string.IsNullOrWhiteSpace(model.Email) && model.Email.Trim() != applicant.Email?.Trim())
                {
                    applicant.Email = model.Email.Trim();
                    hasChanges = true;
                }

                if (!string.IsNullOrWhiteSpace(model.Field_work) && model.Field_work.Trim() != applicant.Field_work?.Trim())
                {
                    applicant.Field_work = model.Field_work.Trim();
                    hasChanges = true;
                }

                if (model.Years_experience.HasValue && model.Years_experience.Value != applicant.Years_experience)
                {
                    applicant.Years_experience = model.Years_experience.Value;
                    hasChanges = true;
                }

                if (!string.IsNullOrWhiteSpace(model.address) && model.address.Trim() != applicant.address?.Trim())
                {
                    applicant.address = model.address.Trim();
                    hasChanges = true;
                }

                // Handle password change only if provided
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    // TODO: Hash the password properly in production
                    applicant.PasswordHash = model.Password;
                    hasChanges = true;
                }

                // Handle file uploads
                if (model.CVFile != null && model.CVFile.Length > 0)
                {
                    var result = await HandleFileUpload(model.CVFile, "cvs", new[] { ".pdf", ".jpg", ".jpeg", ".png" });
                    if (result.Success)
                    {
                        // Delete old CV if exists
                        if (!string.IsNullOrEmpty(applicant.CV))
                        {
                            DeleteOldFile(applicant.CV);
                        }
                        applicant.CV = result.FilePath;
                        hasChanges = true;
                    }
                    else
                    {
                        ModelState.AddModelError("CVFile", result.ErrorMessage);
                        return View(model);
                    }
                }

                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                {
                    var result = await HandleFileUpload(model.ProfileImage, "uploads", new[] { ".jpg", ".jpeg", ".png" });
                    if (result.Success)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(applicant.Profile_image))
                        {
                            DeleteOldFile(applicant.Profile_image);
                        }
                        applicant.Profile_image = result.FilePath;
                        hasChanges = true;
                    }
                    else
                    {
                        ModelState.AddModelError("ProfileImage", result.ErrorMessage);
                        return View(model);
                    }
                }

                // Save changes if any were made
                if (hasChanges)
                {
                    await _context.SaveChangesAsync();
                }

                // Always redirect to Index after successful processing
                return RedirectToAction("Index", new { id = applicant.Id });
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

            var applicant = _context.Applicants.FirstOrDefault(a => a.Id == id);
            if (applicant == null)
                return NotFound();

            var model = new ApplicantEditViewModel
            {
                Id = applicant.Id,
                UserName = applicant.UserName,
                lastName = applicant.lastName,
                PhoneNumber = applicant.PhoneNumber,
                Email = applicant.Email,
                Field_work = applicant.Field_work,
                Years_experience = applicant.Years_experience,
                address = applicant.address,
                CurrentCV = applicant.CV,
                CurrentProfileImage = applicant.Profile_image,

                // Store original values
                OriginalUserName = applicant.UserName,
                OriginalLastName = applicant.lastName,
                OriginalPhoneNumber = applicant.PhoneNumber,
                OriginalEmail = applicant.Email,
                OriginalField_work = applicant.Field_work,
                OriginalYears_experience = applicant.Years_experience,
                OriginalAddress = applicant.address
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
        public IActionResult ViewCV(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var applicant = _context.Applicants.FirstOrDefault(a => a.Id == id);
            if (applicant == null || string.IsNullOrEmpty(applicant.CV))
                return NotFound("Applicant or CV not found.");

            var relativePath = applicant.CV.StartsWith("/") ? applicant.CV.Substring(1) : applicant.CV;
            var fullPath = Path.Combine(_env.WebRootPath, relativePath);

            if (!System.IO.File.Exists(fullPath))
                return NotFound("CV file not found on the server.");

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            var fileExtension = Path.GetExtension(fullPath).ToLowerInvariant();

            // Determine content type based on file extension
            string contentType = fileExtension switch
            {
                ".pdf" => "application/pdf",
                _ => "application/octet-stream"
            };

            var fileName = Path.GetFileName(fullPath);

            // Return file for inline viewing (not as attachment)
            return File(fileBytes, contentType);
        }

        public IActionResult ListApplicants()
        {
            var applicants = _context.Applicants
                .Select(a => new { a.Id, a.UserName, a.Email })
                .ToList();

            return Json(applicants);
        }
    }
}