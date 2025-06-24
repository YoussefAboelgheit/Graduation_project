using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Test1._1.Models.Entity;

namespace Test1._1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDBContext _context;

        public AdminController(AppDBContext context)

        {
            _context = context;
        }

        public IActionResult Dashboard()

        {
            var pendingCompanies = _context.Companies
                 .Where(c => c.status == "Pending")
                  .ToList();

            return View(pendingCompanies);

        }
        [HttpPost]
        public IActionResult AcceptCompany(string id)
        {
            var company = _context.Companies.FirstOrDefault(c => c.Id == id);
            if (company != null)
            {
                company.status = "Accepted";
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult RejectCompany(string id)
        {
            var company = _context.Companies.FirstOrDefault(c => c.Id == id);
            if (company != null)
            {
                company.status = "Rejected";
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }


    }
}

