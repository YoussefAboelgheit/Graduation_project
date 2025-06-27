using Microsoft.AspNetCore.Mvc;
using Test1._1.Models.Entity;
using Test1._1.Models;
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

		// Show available subscriptions
		public IActionResult Index(string companyId)
		{
			if (string.IsNullOrEmpty(companyId))
				return RedirectToAction("Index", "Home");

			TempData["CompanyId"] = companyId;
			TempData.Keep("CompanyId");

			var subs = _context.CompanySubscraptions.ToList();
			return View(subs);
		}

		// Subscribe to selected subscription
		[HttpPost]
		public IActionResult Subscribe(int subId)
		{
			var companyId = TempData["CompanyId"]?.ToString();
			if (string.IsNullOrEmpty(companyId))
				return RedirectToAction("Index", "Home");

			TempData.Keep("CompanyId");

			var subscription = _context.CompanySubscraptions.FirstOrDefault(s => s.Id == subId);
			if (subscription == null)
				return RedirectToAction("Index", new { companyId });

			// Create reference code
			string refCode = "JOB-" + new Random().Next(1000, 9999);

			var transaction = new CompanyTransaction
			{
				CompanyId = companyId,
			CompanySubscraptionId = subId, // ✅ Ensure property name matches model
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

			// Store data in TempData (as strings)
			TempData["RefCode"] = refCode;
			TempData["Amount"] = subscription.Price.ToString();
			TempData["CompanyId"] = companyId;
			TempData.Keep("CompanyId");

			return RedirectToAction("PaymentInstructions");
		}

		// Show payment instructions page
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
