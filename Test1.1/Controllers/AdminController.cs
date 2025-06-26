using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test1._1.Models.Entity;
using Test1._1.Models.ViewModels;

namespace Test1._1.Controllers
{
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

			var companySubscriptions = _context.CompanySubscraptions.ToList();
			var applicantSubscriptions = _context.ApplicantSubscraptions.ToList();

			var pendingCompanyTransactions = _context.CompanyTransactions
				.Include(t => t.Company)
				.Include(t => t.CompanySubscraption)
				.Where(t => !t.IsPaid || !t.IsActive)
				.ToList();

			var pendingApplicantTransactions = _context.ApplicantTransactions
				.Include(t => t.Applicant)
				.Include(t => t.ApplicantSubscraption)
				.Where(t => !t.IsPaid || !t.IsActive)
				.ToList();

			var viewModel = new AdminDashboardViewModel
			{
				Companies = pendingCompanies,
				CompanySubscraptions = companySubscriptions,
				ApplicantSubscraptions = applicantSubscriptions,
				PendingCompanyTransactions = pendingCompanyTransactions,
				PendingApplicantTransactions = pendingApplicantTransactions
			};

			return View(viewModel);
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
		public IActionResult UpdateSubscriptionPrice(int id, decimal newPrice)
		{
			var companySub = _context.CompanySubscraptions.FirstOrDefault(s => s.Id == id);
			if (companySub != null)
			{
				companySub.Price = newPrice;
				_context.SaveChanges();
			}
			else
			{
				var applicantSub = _context.ApplicantSubscraptions.FirstOrDefault(s => s.Id == id);
				if (applicantSub != null)
				{
					applicantSub.Price = newPrice;
					_context.SaveChanges();
				}
			}

			return RedirectToAction("Dashboard");
		}

		[HttpPost]
		public IActionResult ActivateTransaction(int transactionId, string type)
		{
			if (type == "company")
			{
				var trans = _context.CompanyTransactions.FirstOrDefault(t => t.Id == transactionId);
				if (trans != null)
				{
					trans.IsPaid = true;
					trans.IsActive = true;
					_context.SaveChanges();
				}
			}
			else if (type == "applicant")
			{
				var trans = _context.ApplicantTransactions.FirstOrDefault(t => t.Id == transactionId);
				if (trans != null)
				{
					trans.IsPaid = true;
					trans.IsActive = true;
					_context.SaveChanges();
				}
			}

			return RedirectToAction("Dashboard");
		}
	}
}
