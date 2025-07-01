using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Test1._1.Models;
using Test1._1.Models.Entity;
using Test1._1.Models.ViewModels;

namespace Test1._1.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDBContext _context;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment webHostEnvironment, AppDBContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }





        public async Task<IActionResult> Index()
        {

            var activeAds = _context.JobAdvertisments
       .Include(j => j.Company)
       .Where(j => j.IsActive &&
                  !j.IsManuallyDeactivated &&
                  j.ExpiryDate > DateTime.Now)
       .OrderByDescending(j => j.CreatedDate)
       .Select(ad => new CompanyAdvHomeViewModel
       {
           AdvertisementId = ad.Id,
           CompanyId = ad.CompanyId,
           CompanyName = ad.Company.UserName,
           CompanyDescription = ad.Company.Description,
           LogoPath = ad.Company.Logo,
           JobTitle = ad.jobtitle,
           Salary = ad.salary,
           Location = ad.governorate,
           JobTime = ad.Job_time,
           CreatedDate = ad.CreatedDate
       })
       .ToList();

            // كل المستخدمين
            var generalApplicants = _context.Applicants
                .Take(8)
                .Select(a => new ApplicantCardHomeViewModel
                {
                    Id = a.Id,
                    Name = a.UserName,
                    LastName = a.lastName,
                    FieldWork = a.Field_work,
                    ImagePath = a.Profile_image
                })
                .ToList();

            var generalCompanies = _context.Companies
            .Include(c => c.JobAdvertisments)
            .SelectMany(c => c.JobAdvertisments
                .Where(ad => ad.IsActive &&
                            !ad.IsManuallyDeactivated &&
                            ad.ExpiryDate > DateTime.Now)
                .Select(ad => new CompanyAdvHomeViewModel
                {
                    AdvertisementId = ad.Id,
                    CompanyId = c.Id,
                    CompanyName = c.UserName,
                    CompanyDescription = c.Description,
                    LogoPath = c.Logo,
                    JobTitle = ad.jobtitle,
                    Salary = ad.salary.ToString(),
                    Location = ad.governorate,
                    JobTime = ad.Job_time,
                    CreatedDate = ad.CreatedDate
                }))
            .OrderByDescending(ad => ad.CreatedDate)
            .ToList();

            List<CompanyAdvHomeViewModel> applicantSuggestions = new();
            List<ApplicantCardHomeViewModel> companySuggestions = new();

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);

                if (User.IsInRole("Applicant"))
                {
                    var applicant = await _context.Applicants.FirstOrDefaultAsync(a => a.Id == user.Id);

                    var allJobs = _context.JobAdvertisments
                        .Include(j => j.Company)
                        .Where(j => j.IsActive &&
                                   !j.IsManuallyDeactivated &&
                                   j.ExpiryDate > DateTime.Now)
                        .ToList();

                    applicantSuggestions = allJobs
                        .Select(j => new
                        {
                            Job = j,
                            Score =
                                (j.jobtitle.Equals(applicant.Field_work, StringComparison.OrdinalIgnoreCase) ? 50 : 0) +
                                (j.jobtitle.Contains(applicant.Field_work, StringComparison.OrdinalIgnoreCase) ? 20 : 0) +
                                (j.governorate == applicant.address ? 15 : 0) +
                                (j.Job_time == "Full Time" ? 5 : 0) +
                                GetSalaryValue(j.salary) / 1000 +
                                (int)(DateTime.Now - j.CreatedDate).TotalDays * -1 / 2
                        })
                        .OrderByDescending(j => j.Score)
                        .Take(8)
                        .Select(j => new CompanyAdvHomeViewModel
                        {
                            AdvertisementId = j.Job.Id,
                            CompanyId = j.Job.CompanyId,
                            CompanyName = j.Job.Company.UserName,
                            CompanyDescription = j.Job.Company.Description,
                            LogoPath = j.Job.Company.Logo,
                            JobTitle = j.Job.jobtitle,
                            Salary = j.Job.salary,
                            Location = j.Job.governorate,
                            JobTime = j.Job.Job_time,
                            CreatedDate = j.Job.CreatedDate
                        })
                        .ToList();
                }
                else if (User.IsInRole("Company"))
                {
                    var company = await _context.Companies
                        .Include(c => c.JobAdvertisments)
                        .FirstOrDefaultAsync(c => c.Id == user.Id);

                    var recentJobTitles = company.JobAdvertisments
                        .OrderByDescending(j => j.CreatedDate)
                        .Take(8)
                        .Select(j => j.jobtitle)
                        .ToList();

                    var allApplicants = _context.Applicants.ToList();

                    companySuggestions = allApplicants
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
                        .Take(9)
                        .Select(a => new ApplicantCardHomeViewModel
                        {
                            Id = a.Applicant.Id,
                            Name = a.Applicant.UserName,
                            LastName = a.Applicant.lastName,
                            FieldWork = a.Applicant.Field_work,
                            ImagePath = a.Applicant.Profile_image
                        })
                        .ToList();
                }
            }

            var viewModel = new HomeViewModel
            {
                Applicants = generalApplicants,
                ApplicantSuggestions = applicantSuggestions,
                CompanySuggestions = companySuggestions,
                Companies = activeAds
            };

            return View(viewModel);
        }

        [HttpGet]
		public IActionResult FilterApplicants(string governorate, string experience, string job)
		{
			var query = _context.Applicants.AsQueryable();

			if (!string.IsNullOrEmpty(governorate))
				query = query.Where(a => a.address == governorate);

			if (!string.IsNullOrEmpty(experience))
			{
				if (experience.StartsWith(">"))
				{
					if (int.TryParse(experience.Substring(1), out int years))
					{
						query = query.Where(a => a.Years_experience > years);
					}
				}
				else if (int.TryParse(experience, out int exactYears))
				{
					query = query.Where(a => a.Years_experience == exactYears);
				}
			}

			if (!string.IsNullOrEmpty(job))
			{
				if (job == "Other")
				{
					// Known job options from the dropdown
					var knownJobs = new List<string>
		{
			"AI Engineer",
			"AR/VR Developer",
			"Back‑End Developer",
			"Blockchain Developer",
			"Cloud Engineer",
			"Cybersecurity Specialist",
			"Data Analyst",
			"Data Scientist",
			"Database Administrator",
			"DevOps Engineer",
			"Embedded Systems Engineer",
			"Front‑End Developer",
			"Full Stack Developer",
			"Game Developer",
			"IT Project Manager",
			"IT Support Specialist",
			"Machine Learning Engineer",
			"Mobile App Developer",
			"Network Engineer",
			"QA/Test Engineer",
			"Software Engineer",
			"System Administrator",
			"Technical Writer",
			"UI/UX Designer"
		};

					// Get only applicants whose Field_work is not in the known jobs
					query = query.Where(a => !knownJobs.Contains(a.Field_work));
				}
				else
				{
					// Regular job match
					query = query.Where(a => a.Field_work == job);
				}
			}

			var applicants = query
				.Select(a => new ApplicantCardHomeViewModel
				{
					Id = a.Id,
					Name = a.UserName,
					LastName = a.lastName,
					FieldWork = a.Field_work,
					ImagePath = a.Profile_image
				})
				.ToList();

			return PartialView("_ApplicantList", applicants);
		}




        [HttpGet]
        public IActionResult FilterAdvertisements(string governorate, string job, string salary)
        {
            var query = _context.JobAdvertisments
                .Include(ad => ad.Company)
                .Where(ad => ad.IsActive &&
                !ad.IsManuallyDeactivated &&
                ad.ExpiryDate > DateTime.Now)
                .AsQueryable();

            // Filter by governorate (city)
            if (!string.IsNullOrEmpty(governorate))
                query = query.Where(ad => ad.governorate == governorate);

            // Filter by job title
            if (!string.IsNullOrEmpty(job))
            {
                if (job == "Others")
                {
                    var knownJobs = new List<string>
            {
                "AI Engineer", "AR/VR Developer", "Back‑End Developer", "Blockchain Developer",
                "Cloud Engineer", "Cybersecurity Specialist", "Data Analyst", "Data Scientist",
                "Database Administrator", "DevOps Engineer", "Embedded Systems Engineer",
                "Front‑End Developer", "Full Stack Developer", "Game Developer", "IT Project Manager",
                "IT Support Specialist", "Machine Learning Engineer", "Mobile App Developer",
                "Network Engineer", "QA/Test Engineer", "Software Engineer", "System Administrator",
                "Technical Writer", "UI/UX Designer"
            };
                    query = query.Where(ad => !knownJobs.Contains(ad.jobtitle));
                }
                else
                {
                    query = query.Where(ad => ad.jobtitle == job);
                }
            }

            // Filter by salary directly in the database query for exact matching
            if (!string.IsNullOrEmpty(salary))
            {
                query = query.Where(ad => ad.salary == salary);
            }

            var advertisements = query
                .OrderByDescending(ad => ad.CreatedDate)
                .Select(ad => new CompanyAdvHomeViewModel
                {
                    AdvertisementId = ad.Id,
                    CompanyId = ad.CompanyId,
                    CompanyName = ad.Company.UserName,
                    CompanyDescription = ad.Company.Description,
                    LogoPath = ad.Company.Logo,
                    JobTitle = ad.jobtitle,
                    Salary = ad.salary,
                    Location = ad.governorate,
                    JobTime = ad.Job_time,
                    CreatedDate = ad.CreatedDate,
                    JobDescription = ad.Jobdetail,
                    Requirements = ad.JobRequirements
                })
                .ToList();

            ViewBag.header = "All Advertisements";
            return PartialView("_CompanyAdList", advertisements);
        }


        [HttpGet]
		public async Task<IActionResult> Suggestions(string q)
		{
			if (string.IsNullOrWhiteSpace(q))
			{
				return Json(new
				{
					fieldOfWork = new List<object>(),
					city = new List<object>(),
					username = new List<object>()
				});
			}

			var lower = q.ToLower();

			// Get Applicant and Ad field of work
			var fieldOfWorkApplicants = await _context.Applicants
				.Where(a => a.Field_work.ToLower().Contains(lower))
				.Select(a => new
				{
					label = a.Field_work,
					sub = a.address,
					type = "applicantField",
					id = a.Id.ToString(),
					source = "applicant"
				})
				.Take(5)
				.ToListAsync();

			var fieldOfWorkAds = await _context.JobAdvertisments
				.Where(ad => ad.jobtitle.ToLower().Contains(lower))
				.Select(ad => new
				{
					label = ad.jobtitle,
					sub = ad.governorate,
					type = "adField",
					id = ad.Id.ToString(),
					source = "ad"
				})
				.Take(5)
				.ToListAsync();

			// Get Applicant and Ad cities
			var applicantCities = await _context.Applicants
				.Where(a => a.address.ToLower().Contains(lower))
				.Select(a => new
				{
					label = a.address,
					sub = a.Field_work,
					type = "applicantCity",
					id = a.Id.ToString(),
					source = "applicant"
				})
				.Take(5)
				.ToListAsync();

			var adCities = await _context.JobAdvertisments
				.Where(ad => ad.governorate.ToLower().Contains(lower))
				.Select(ad => new
				{
					label = ad.governorate,
					sub = ad.jobtitle,
					type = "adCity",
					id = ad.Id.ToString(),
					source = "ad"
				})
				.Take(5)
				.ToListAsync();

			// Get Usernames: separate companies vs applicants
			var applicantIds = await _context.Applicants.Select(a => a.Id).ToListAsync();
			var companyIds = await _context.Companies.Select(c => c.Id).ToListAsync();

			var usernames = await _context.Users
				.Where(u => u.UserName.ToLower().Contains(lower))
				.Select(u => new
				{
					label = u.UserName,
					sub = "",
					type = "user",
					id = u.Id.ToString(),
					source = applicantIds.Contains(u.Id) ? "applicant" :
							 companyIds.Contains(u.Id) ? "company" : "unknown"
				})
				.Take(10)
				.ToListAsync();

			return Json(new
			{
				fieldOfWork = fieldOfWorkApplicants.Concat(fieldOfWorkAds),
				city = applicantCities.Concat(adCities),
				username = usernames
			});
		}



        [HttpGet]
        public IActionResult AllApplicants()
        {
            var applicants = _context.Applicants
                .Select(a => new ApplicantCardHomeViewModel
                {
                    Id = a.Id,
                    Name = a.UserName,
                    LastName = a.lastName,
                    FieldWork = a.Field_work,
                    ImagePath = a.Profile_image
                })
                .ToList();

            return View(applicants);
        }







        public IActionResult Privacy()
        {
            return View();
        }

        // Add this method to your HomeController.cs

        // Add this method to your HomeController.cs

        [HttpPost]
        public async Task<IActionResult> CheckEmailExists([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { exists = false });
            }

            // Check if email exists in database
            bool emailExists = await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower() && !u.IsDeleted);

            return Json(new { exists = emailExists });
        }


        [HttpGet]
        public IActionResult SignUp()
        {
            return View(new SignUpViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompanySignUp(CompanySignUpViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // ✅ Validate file extensions for uploaded files
                    bool IsValidFileLogo(IFormFile file)
                    {
                        if (file == null || file.Length == 0)
                            return false;

                        string extension = Path.GetExtension(file.FileName);
                        return Regex.IsMatch(extension, @"\.(jpg|jpeg|png)$", RegexOptions.IgnoreCase);
                    }
                    bool IsValidFilePDF(IFormFile file)
                    {
                        if (file == null || file.Length == 0)
                            return false;

                        string extension = Path.GetExtension(file.FileName);
                        return Regex.IsMatch(extension, @"\.(pdf)$", RegexOptions.IgnoreCase);
                    }

                    if (!IsValidFileLogo(model.Logo))
                    {
                        ModelState.AddModelError("Logo", "Logo must have a valid file extension: .jpg, .jpeg, or .png");
                    }
                    if (!IsValidFilePDF(model.TaxCard))
                    {
                        ModelState.AddModelError("TaxCard", "Tax card must be a .pdf extension");
                    }
                    if (!IsValidFilePDF(model.CommercialRegister))
                    {
                        ModelState.AddModelError("CommercialRegister", "Commercial register must be a .pdf extension");
                    }

                    //❌ If any file validation fails, return view with errors
                    if (!ModelState.IsValid)
                    {
                        return View("SignUp", new SignUpViewModel
                        {
                            UserName = model.UserName,
                            Email = model.Email,
                            Password = model.Password,
                            ConfirmPassword = model.ConfirmPassword,
                            Phone = model.Phone,
                            FiledWork = model.FiledWork,
                            Address = model.Address,
                            Logo = model.Logo,
                            TaxCard = model.TaxCard,
                            CommercialRegister = model.CommercialRegister,
                            Description = model.Description
                        });
                    }

                    // ✅ Ensure uploads folder exists
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    // ✅ Function to handle file saving
                    string SaveFile(IFormFile? file)
                    {
                        if (file == null || file.Length == 0)
                            return null; // Return null if no file uploaded

                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        return Path.Combine("uploads", fileName); // Relative path
                    }

                    // ✅ Process file uploads
                    string logoPath = SaveFile(model.Logo);
                    string taxCardPath = SaveFile(model.TaxCard);
                    string commercialRegisterPath = SaveFile(model.CommercialRegister);

                    // ✅ Create new Company object
                    Company company = new Company
                    {
                        UserName = model.UserName,
                        PhoneNumber = model.Phone,
                        Email = model.Email,
                        Logo = logoPath,
                        FiledWork = model.FiledWork,
                        TaxCard = taxCardPath,
                        CommercialRegister = commercialRegisterPath,
                        Description = model.Description,
                        address = model.Address
                    };

                    var result = await _userManager.CreateAsync(company, model.Password);
                    if (result.Succeeded)
                    {
                        // ✅ Add to role
                        if (!await _roleManager.RoleExistsAsync("Company"))
                            await _roleManager.CreateAsync(new IdentityRole("Company"));

                        await _userManager.AddToRoleAsync(company, "Company");

                        // ✅ Sign in the user (create cookie)
                        await _signInManager.SignInAsync(company, isPersistent: false);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again.");
                // You might want to log the actual exception details here
            }

            return View("SignUp", new SignUpViewModel
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                Phone = model.Phone,
                FiledWork = model.FiledWork,
                Address = model.Address,
                Logo = model.Logo,
                TaxCard = model.TaxCard,
                CommercialRegister = model.CommercialRegister,
                Description = model.Description
            });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplicantSignUp(ApplicantSignUpViewModel model)
        {
            try
            {
                // Handle the "Other" field work selection
                if (model.Field_work == "Other" && !string.IsNullOrWhiteSpace(model.Field_work_other))
                {
                    model.Field_work = model.Field_work_other.Trim();
                }

                // Remove Field_work_other from ModelState validation if not needed
                ModelState.Remove("Field_work_other");

                if (ModelState.IsValid)
                {
                    // cv must be pdf
                    bool IsValidCV(IFormFile file)
                    {
                        if (file == null || file.Length == 0)
                            return false;

                        string extension = Path.GetExtension(file.FileName);
                        return Regex.IsMatch(extension, @"\.(pdf)$", RegexOptions.IgnoreCase);
                    }

                    if (!IsValidCV(model.CVFile))
                    {
                        ModelState.AddModelError("CVFile", "CV must be in .pdf format.");
                    }

                    // ProfileImage must be image
                    bool IsValidProfileImage(IFormFile file)
                    {
                        if (file == null || file.Length == 0)
                            return false;

                        string extension = Path.GetExtension(file.FileName);
                        return Regex.IsMatch(extension, @"\.(jpg|jpeg|png)$", RegexOptions.IgnoreCase);
                    }

                    if (!IsValidProfileImage(model.ProfileImage))
                    {
                        ModelState.AddModelError("ProfileImage", "Profile image must be in .jpg, .jpeg, or .png format.");
                    }

                    if (!ModelState.IsValid)
                    {
                        return View("SignUp", new SignUpViewModel
                        {
                            Fname = model.Fname,
                            Lname = model.Lname,
                            Email = model.Email,
                            Password = model.Password,
                            ConfirmPassword = model.ConfirmPassword,
                            Phone = model.Phone,
                            Field_work = model.Field_work,
                            Years_experience = model.Years_experience,
                            Address = model.Address,
                            CVFile = model.CVFile,
                            ProfileImage = model.ProfileImage
                        });
                    }

                    // ✅ Ensure uploads folder exists
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    // ✅ Function to save file and return path
                    string SaveFile(IFormFile? file)
                    {
                        if (file == null || file.Length == 0)
                            return null;

                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        return Path.Combine("uploads", fileName);
                    }

                    // ✅ Save files
                    string cvPath = SaveFile(model.CVFile);
                    string ProfileImagePath = SaveFile(model.ProfileImage);

                    // ✅ Create Applicant object
                    Applicant applicant = new Applicant
                    {
                        UserName = model.Fname,
                        lastName = model.Lname,
                        PhoneNumber = model.Phone,
                        Email = model.Email,
                        Field_work = model.Field_work, // This will now contain the custom value if "Other" was selected
                        Years_experience = model.Years_experience,
                        address = model.Address,
                        CV = cvPath,
                        Profile_image = ProfileImagePath
                    };

                    var result = await _userManager.CreateAsync(applicant, model.Password);
                    if (result.Succeeded)
                    {
                        // ✅ Ensure "Applicant" role exists
                        if (!await _roleManager.RoleExistsAsync("Applicant"))
                            await _roleManager.CreateAsync(new IdentityRole("Applicant"));

                        await _userManager.AddToRoleAsync(applicant, "Applicant");

                        // ✅ Sign In user
                        await _signInManager.SignInAsync(applicant, isPersistent: false);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again.");
                // You might want to log the actual exception details here
            }

            // If we get here, something failed, return to signup with a new SignUpViewModel
            return View("SignUp", new SignUpViewModel
            {
                Fname = model.Fname,
                Lname = model.Lname,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                Phone = model.Phone,
                Field_work = model.Field_work,
                Years_experience = model.Years_experience,
                Address = model.Address,
                CVFile = model.CVFile,
                ProfileImage = model.ProfileImage
            });
        }


        [HttpGet]
        public IActionResult SignIn()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckSignIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "No account found with this email.");
                    return View("SignIn", model);
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else
                    {
                    return RedirectToAction("Index", "Home");
                }
                }

                ModelState.AddModelError(string.Empty, "Invalid password.");
            }

            return View("SignIn", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private int GetSalaryValue(string salaryRange)
        {
            return salaryRange switch
            {
                "2000 - 4000" => 3000,
                "4000 - 6000" => 5000,
                "6000 - 8000" => 7000,
                "8000 - 10000" => 9000,
                "More than 10000" => 11000,
                "More than 15000" => 16000,
                "More than 20000" => 21000,
                _ => 0
            };
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Sections(string job)
        {
            var query = _context.JobAdvertisments
                .Include(ad => ad.Company)
                .Where(ad => ad.IsActive &&
                            !ad.IsManuallyDeactivated &&
                            ad.ExpiryDate > DateTime.Now)
                .AsQueryable();

            if (!string.IsNullOrEmpty(job))
            {
                if (job == "Others")
                {
                    var knownJobs = new List<string>
            {
                "AI Engineer", "AR/VR Developer", "Back‑End Developer", "Blockchain Developer",
                "Cloud Engineer", "Cybersecurity Specialist", "Data Analyst", "Data Scientist",
                "Database Administrator", "DevOps Engineer", "Embedded Systems Engineer",
                "Front‑End Developer", "Full Stack Developer", "Game Developer", "IT Project Manager",
                "IT Support Specialist", "Machine Learning Engineer", "Mobile App Developer",
                "Network Engineer", "QA/Test Engineer", "Software Engineer", "System Administrator",
                "Technical Writer", "UI/UX Designer"
            };
                    query = query.Where(ad => !knownJobs.Contains(ad.jobtitle));
                }
                else
                {
                    query = query.Where(ad => ad.jobtitle == job);
                }
            }

            var advertisements = query
                .OrderByDescending(ad => ad.CreatedDate)
                .Select(ad => new CompanyAdvHomeViewModel
                {
                    AdvertisementId = ad.Id,
                    CompanyId = ad.CompanyId,
                    CompanyName = ad.Company.UserName,
                    CompanyDescription = ad.Company.Description,
                    LogoPath = ad.Company.Logo,
                    JobTitle = ad.jobtitle,
                    Salary = ad.salary,
                    Location = ad.governorate,
                    JobTime = ad.Job_time,
                    CreatedDate = ad.CreatedDate,
                    JobDescription = ad.Jobdetail,
                    Requirements = ad.JobRequirements
                })
                .ToList();

            ViewBag.header = "All " + job + " Advertisements";
            return View("AllAdvertisements", advertisements);
        }


    }
}