using Microsoft.AspNetCore.Mvc;
using Test1._1.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Test1._1.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Test1._1.Controllers
{
    public class JobAdvertisementController : Controller
    {
        private readonly AppDBContext _context;

        public JobAdvertisementController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Create new advertisement form
        public IActionResult Create(string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Home");
            }

            var company = _context.Companies.FirstOrDefault(c => c.Id == companyId);
            if (company == null)
            {
                return NotFound();
            }

            var model = new JobAdvertisementViewModel
            {
                CompanyId = companyId
            };

            return View(model);
        }

        // POST: Handle form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobAdvertisementViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Create the job advertisement
                var jobAd = new JobAdvertisment
                {
                    jobtitle = model.FieldWork,
                    Jobdetail = model.JobDescription,
                    //NumEmployee = 1, // Default value
                    Job_time = model.JobTime,
                    governorate = model.City,
                    salary = model.Salary,
                    CompanyId = model.CompanyId,
                    JobRequirements = model.Job_Requirements,
                    CreatedDate = DateTime.Now
                };

                _context.JobAdvertisments.Add(jobAd);
                await _context.SaveChangesAsync();

                // Create custom questions
                if (model.CustomQuestions != null && model.CustomQuestions.Any())
                {
                    foreach (var questionText in model.CustomQuestions)
                    {
                        if (!string.IsNullOrWhiteSpace(questionText))
                        {
                            var question = new Question
                            {
                                Text = questionText.Trim(),
                                Type = "text",
                                IsShared = false,
                                JobAdvertismentId = jobAd.Id
                            };
                            _context.Questions.Add(question);
                        }
                    }

                    int questionsSaved = await _context.SaveChangesAsync();
                }

                // Redirect back to company profile
                return RedirectToAction("Index", "CompanyInfo", new { id = model.CompanyId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the job advertisement. Please try again.");
                return View(model);
            }
        }

        [Authorize]
        // GET: View advertisement details
        public async Task<IActionResult> Form(int id)
        {
            var jobAd = await _context.JobAdvertisments
                .Include(j => j.Company)
                .Include(j => j.Questions)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jobAd == null)
            {
                return NotFound();
            }

            // Get current user's ID
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Check if user is an applicant OR the company owner of this job ad
            bool isApplicant = User.IsInRole("Applicant");
            bool isCompanyOwner = jobAd.CompanyId == currentUserId;

            if (!isApplicant && !isCompanyOwner)
            {
                return Forbid(); // Return 403 Forbidden if user is neither applicant nor company owner
            }

            return View(jobAd);
        }


        public async Task<IActionResult> Details(int id)
        {
            var jobAd = await _context.JobAdvertisments
                .Include(j => j.Company)
                .Include(j => j.Questions)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jobAd == null)
            {
                return NotFound();
            }

            return View(jobAd);
        }


        // Test action to debug database content
        public async Task<IActionResult> TestData(int id)
        {
            var jobAd = await _context.JobAdvertisments
                .Include(j => j.Company)
                .Include(j => j.Questions)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jobAd == null)
                return Json(new { error = "Job not found" });

            return Json(new
            {
                id = jobAd.Id,
                title = jobAd.jobtitle,
                salary = jobAd.salary,
                city = jobAd.governorate,
                jobTime = jobAd.Job_time,
                requirements = jobAd.JobRequirements,
                description = jobAd.Jobdetail,
                companyName = jobAd.Company?.UserName,
                questionsCount = jobAd.Questions?.Count ?? 0,
                questions = jobAd.Questions?.Select(q => new { id = q.Id, text = q.Text, type = q.Type }).ToList()
            });
        }
    }
}