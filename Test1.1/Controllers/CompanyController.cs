using Microsoft.AspNetCore.Mvc;
using Test1._1.Models.Entity;
using Test1._1.Models.ViewModels;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Test1._1.Controllers
{
    public class CompanyController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CompanyController(AppDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        //[HttpGet]
        //public IActionResult SignUp()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SignUp(CompanySignUpViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Hash password
        //        string hashedPassword = HashPassword(model.Password);

        //        // Handle logo upload
        //        string logoPath = null;
        //        if (model.Logo != null)
        //        {
        //            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        //            logoPath = Path.Combine(uploadsFolder, model.Logo.FileName);
        //            using (var stream = new FileStream(logoPath, FileMode.Create))
        //            {
        //                await model.Logo.CopyToAsync(stream);
        //            }
        //        }

        //        // Create new Company object
        //        Company company = new Company
        //        {
        //            Fname = model.Fname,
        //            Lname = model.Lname,
        //            HashedPassword = hashedPassword,
        //            Phone = model.Phone,
        //            Email = model.Email,
        //            Logo = logoPath,
        //            FiledWork = model.FiledWork,
        //            TaxCard = model.TaxCard,
        //            CommercialRegister = model.CommercialRegister,
        //            Description = model.Description
        //        };

        //        _context.Companies.Add(company);
        //        await _context.SaveChangesAsync();

        //        return RedirectToAction("Login", "Account");
        //    }
        //    return View(model);
        //}
        

        //private string HashPassword(string password)
        //{
        //    using (SHA256 sha256 = SHA256.Create())
        //    {
        //        byte[] bytes = Encoding.UTF8.GetBytes(password);
        //        byte[] hash = sha256.ComputeHash(bytes);
        //        return Convert.ToBase64String(hash);
        //    }
        //}
    }
}
