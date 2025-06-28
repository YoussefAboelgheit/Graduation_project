using Microsoft.AspNetCore.Mvc;
using Test1._1.Models.Entity;
using System;
using System.Linq;

namespace Test1._1.Controllers
{
	public class CompanySubscraptionsController : Controller
	{
		private readonly AppDBContext _context;

		public CompanySubscraptionsController(AppDBContext context)
		{
			_context = context;
		}

		[HttpPost]
		public IActionResult Subscribe(int subId, string companyId)
		{
			if (string.IsNullOrEmpty(companyId))
				return RedirectToAction("Index", "Home");

			var subscription = _context.CompanySubscraptions.FirstOrDefault(s => s.Id == subId);
			if (subscription == null)
				return RedirectToAction("Index", "CompanyInfo", new { id = companyId });

			// إنشاء كود مرجعي عشوائي
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
	}
}
