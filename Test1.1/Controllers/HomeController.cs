using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test1._1.Models;
using Test1._1.Models.Entity;
using Test1._1.Models.ViewModels;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Test1._1.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(AppDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
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
                    return View(model);
                }

                // ✅ Hash password
                string hashedPassword = HashPassword(model.Password);

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
                    Fname = model.Fname,
                    Lname = model.Lname,
                    HashedPassword = hashedPassword,
                    Phone = model.Phone,
                    Email = model.Email,
                    Logo = logoPath,
                    FiledWork = model.FiledWork,
                    TaxCard = taxCardPath,
                    CommercialRegister = commercialRegisterPath,
                    Description = model.Description
                };

                _context.Companies.Add(company);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return View("SignUp",model);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplicantSignUp(ApplicantSignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                // cv must be pdf or image
                bool IsValidCV(IFormFile file)
                {
                    if (file == null || file.Length == 0)
                        return false;

                    string extension = Path.GetExtension(file.FileName);
                    return Regex.IsMatch(extension, @"\.(pdf|jpg|jpeg|png)$", RegexOptions.IgnoreCase);
                }

                if (!IsValidCV(model.CVFile))
                {
                    ModelState.AddModelError("CVFile", "CV must be in .pdf, .jpg, .jpeg, or .png format.");
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                string hashedPassword = HashPassword(model.Password);

                // ✅ التأكد من وجود مجلد التخزين
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                // ✅ دالة لحفظ الملف وإرجاع المسار
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

                // ✅ حفظ الـ CV
                string cvPath = SaveFile(model.CVFile);

                // ✅ إنشاء كائن `Applicant`
                Applicant applicant = new Applicant
                {
                    Fname = model.Fname,
                    Lname = model.Lname,
                    HashedPassword = hashedPassword,
                    Phone = model.Phone,
                    Email = model.Email,
                    Field_work = model.Field_work,
                    Years_experience = model.Years_experience,
                    CV = cvPath
                };

                _context.Applicants.Add(applicant);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View("SignUp", model);
        }



        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }




        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckSignIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = HashPassword(model.HashedPassword);
                var user = _context.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.HashedPassword == hashedPassword && !u.IsDeleted);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View("SignIn", model);
                }

                
                return RedirectToAction("Index");
            }

            return View("SignIn", model);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
