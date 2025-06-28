using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using Test1._1.Models.Entity;
using Test1._1.Models.ViewModels;

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

		public IActionResult Index(string id)
		{
			if (string.IsNullOrEmpty(id))
				return RedirectToAction("ErrorPage");

			var company = _context.Companies.FirstOrDefault(c => c.Id == id);
			if (company == null)
				return NotFound();

			// جلب الاشتراكات
			var subs = _context.CompanySubscraptions.ToList();
			ViewBag.CompanySubscraptions = subs;

			// جلب الاشتراك المفعّل الحالي
			var activeTransaction = _context.CompanyTransactions
				.Include(t => t.CompanySubscraption)
				.FirstOrDefault(t => t.CompanyId == company.Id && t.IsPaid && t.IsActive);

			ViewBag.ActiveTransaction = activeTransaction;

			return View(company);
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
	}
}
