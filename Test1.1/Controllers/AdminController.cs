using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test1._1.Models.Configration;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using Test1._1.Models.Entity;
using Test1._1.Models.ViewModels;
using Microsoft.Extensions.Options;

namespace Test1._1.Controllers
{
    [Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly AppDBContext _context;
        private readonly SmtpSettings _smtpSettings;

        public AdminController(AppDBContext context, IOptions<SmtpSettings> smtpOptions)
		{
			_context = context;
            _smtpSettings = smtpOptions.Value;
		}

		public IActionResult Dashboard()

		{
			var pendingCompanies = _context.Companies
				.Where(c => c.status == "Pending")
                .Select(c => new CompanyViewModel
                {
                    Id = c.Id,
                    UserName = c.UserName,
                    Logo = c.Logo,
                    FiledWork = c.FiledWork,
                    CurrentNumEmployees = c.CurrentNumEmployees,
                    Description = c.Description,
                    TaxCard = c.TaxCard,
                    CommercialRegister = c.CommercialRegister
                })
				.ToList();

			var companySubscriptions = _context.CompanySubscraptions.ToList();
			var applicantSubscriptions = _context.ApplicantSubscraptions.ToList();

            return View(pendingCompanies);

        }
        [HttpGet]
        public IActionResult CompanyDetails(string id) 
        {
            var company = _context.Companies
                //.Take(5)
                .Where(c => c.Id == id)
                .Select(c => new CompanyViewModel {
                    Id = c.Id,
                    UserName = c.UserName,
                    Logo = c.Logo,
                    FiledWork = c.FiledWork,
                    CurrentNumEmployees = c.CurrentNumEmployees,
                    Description = c.Description,
                    TaxCard = c.TaxCard,
                    CommercialRegister = c.CommercialRegister
                })
                .SingleOrDefault();
            if (company == null)
                return NotFound();

            return PartialView("_CompanyCardPartial",company);
        }


        [HttpGet]
        public IActionResult CompanyCheckout(string id)
        {
            var company = _context.Companies
                .Where(c => c.Id == id)
                .Select(c => new CompanyViewModel
                {
                    Id = c.Id,
                    UserName = c.UserName,
                    Logo = c.Logo,
                    FiledWork = c.FiledWork,
                    CurrentNumEmployees = c.CurrentNumEmployees,
                    Description = c.Description,
                    TaxCard = c.TaxCard,
                    CommercialRegister = c.CommercialRegister
                })
                .SingleOrDefault();

            if (company == null)
                return NotFound();

            return View(company); 
        }
        [HttpGet]
        public IActionResult CompaniesPending() 
        {
            ViewBag.StatusTitle = "Pending Companies";
            var pendingCompanies = _context.Companies
                .Where(c => c.status == "Pending")
                .Select(c => new CompanyViewModel
                {
                    Id = c.Id,
                    UserName = c.UserName,
                    Logo = c.Logo,
                    FiledWork = c.FiledWork,
                    CurrentNumEmployees = c.CurrentNumEmployees,
                    Description = c.Description,
                    TaxCard = c.TaxCard,
                    CommercialRegister = c.CommercialRegister
                })
				.ToList();


            return View(pendingCompanies);
        }
        public IActionResult CompaniesAccept()
        {
            ViewBag.StatusTitle = "Accepted Companies";
            var acceptedCompanies = _context.Companies
                .Where(c => c.status == "Accepted")
                .Select(c => new CompanyViewModel
                {
                    Id = c.Id,
                    UserName = c.UserName,
                    Logo = c.Logo,
                    FiledWork = c.FiledWork,
                    CurrentNumEmployees = c.CurrentNumEmployees,
                    Description = c.Description,
                    TaxCard = c.TaxCard,
                    CommercialRegister = c.CommercialRegister
                })
				.ToList();

            return View("CompaniesPending", acceptedCompanies); // ✅ نفس الفيو
        }

        public IActionResult CompaniesReject()
        {
            ViewBag.StatusTitle = "Rejected Companies";
            var rejectedCompanies = _context.Companies
                .Where(c => c.status == "Rejected")
                .Select(c => new CompanyViewModel
			{
                    Id = c.Id,
                    UserName = c.UserName,
                    Logo = c.Logo,
                    FiledWork = c.FiledWork,
                    CurrentNumEmployees = c.CurrentNumEmployees,
                    Description = c.Description,
                    TaxCard = c.TaxCard,
                    CommercialRegister = c.CommercialRegister
                })
                .ToList();

            return View("CompaniesPending", rejectedCompanies); // ✅ نفس الفيو
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
        [HttpGet]
        public IActionResult RejectForm(string id)
        {
            var company = _context.Companies.FirstOrDefault(c => c.Id == id);
            if (company == null)
                return NotFound();

            var model = new RejectCompanyViewModel
			{
                CompanyId = company.Id,
                CompanyEmail = company.Email,
                CompanyName = company.UserName
            };

            return View(model);
			}

        [HttpPost]
        public async Task<IActionResult> RejectForm(RejectCompanyViewModel model)
				{
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == model.CompanyId);
            if (company == null)
                return NotFound();

            // 1. إرسال الإيميل
            string subject = "Company Rejection Notification";
            // HTML body
            var body = $@"
                    <p>Dear <strong>{model.CompanyName}</strong>,</p>

                    <p>Thank you for registering with <strong>Jobify</strong>.</p>

                    <p>We regret to inform you that your company registration has been <strong style='color:red;'>rejected</strong> for the following reason:</p>

                    <blockquote style='color: #b30000; font-style: italic;'>
                        {model.ReportMessage}
                    </blockquote>

                    <p>If you address the mentioned issues or make the necessary changes, you are welcome to contact us for further review.</p>

                    <p>Best regards,<br/>
                    <strong>The Jobify Team</strong></p>
                    ";

            await SendEmailAsync(model.CompanyEmail, subject, body);


            // 2. تحديث حالة الشركة
            company.status = "Rejected";
            await _context.SaveChangesAsync();

			return RedirectToAction("Dashboard");
		}

        private async Task SendEmailAsync(string toEmail, string subject, string body)
				{
            var smtpClient = new SmtpClient(_smtpSettings.Host)
			{
                Port = _smtpSettings.Port,
                Credentials = new NetworkCredential(_smtpSettings.Email, _smtpSettings.AppPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
				{
                From = new MailAddress(_smtpSettings.Email, "Jobify Team"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
			}


	}
}

