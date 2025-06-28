using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Test1._1.Models.Entity;
using Test1._1.Models.ViewModels;

namespace Test1._1.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDBContext _context;

        public AdminController(AppDBContext context)

        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = new AdminDashboardViewModel
            {
                PendingCompanies = await _context.Companies
                    .Where(c => c.status == "Pending")
                    .ToListAsync(),
                PendingEdits = await _context.EditAdvertisments
                    .Include(e => e.JobAdvertisment)
                    .Where(e => e.Status == "Pending")
                    .ToListAsync()
            };

            return View(model);
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


        [HttpPost]
        public async Task<IActionResult> ApproveEdit(int editId)
        {
            var edit = await _context.EditAdvertisments
                .Include(e => e.JobAdvertisment)
                .FirstOrDefaultAsync(e => e.Id == editId);

            if (edit == null)
            {
                return NotFound();
            }

            // Apply changes to the job ad
            var jobAd = edit.JobAdvertisment;
            jobAd.jobtitle = edit.JobTitle;
            jobAd.Jobdetail = edit.JobDetail;
            jobAd.Job_time = edit.JobTime;
            jobAd.governorate = edit.Governorate;
            jobAd.salary = edit.Salary;
            jobAd.JobRequirements = edit.JobRequirements;
            jobAd.HasPendingEdits = false;

            edit.Status = "Approved";
            edit.EditDate = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["EditApproved"] = "The edit has been approved and applied to the advertisement.";
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> RejectEdit(int editId)
        {
            var edit = await _context.EditAdvertisments
                .Include(e => e.JobAdvertisment)
                .FirstOrDefaultAsync(e => e.Id == editId);

            if (edit == null)
            {
                return NotFound();
            }

            edit.Status = "Rejected";
            edit.EditDate = DateTime.Now;
            edit.JobAdvertisment.HasPendingEdits = false;

            await _context.SaveChangesAsync();

            TempData["EditRejected"] = "The edit has been rejected.";
            return RedirectToAction("Dashboard");
        }


    }
}