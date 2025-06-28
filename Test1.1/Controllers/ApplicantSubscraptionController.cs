using Microsoft.AspNetCore.Mvc;
using Test1._1.Models.Entity;
using System;
using System.Linq;

namespace Test1._1.Controllers
{
	public class ApplicantSubscrptionController : Controller
	{
		private readonly AppDBContext _context;

		public ApplicantSubscrptionController(AppDBContext context)
		{
			_context = context;
		}

		[HttpPost]
		public IActionResult Subscribe(int subId, string applicantId)
		{
			if (string.IsNullOrEmpty(applicantId))
				return RedirectToAction("Index", "Home");

			var subscription = _context.ApplicantSubscraptions.FirstOrDefault(s => s.Id == subId);
			if (subscription == null)
				return RedirectToAction("Index", "ApplicantInfo", new { id = applicantId });

			// Generate reference code
			string refCode = "APP-" + new Random().Next(1000, 9999);

			// Calculate end date based on SubType
			DateTime startDate = DateTime.Now;
			DateTime endDate = subscription.SubType.ToLower() switch
			{
				"daily" => startDate.AddDays(1),
				"weekly" => startDate.AddDays(7),
				"monthly" => startDate.AddMonths(1),
				"yearly" => startDate.AddYears(1),
				_ => startDate.AddDays(7) // default fallback
			};

			var transaction = new ApplicantTransaction
			{
				ApplicantId = applicantId,
				ApplicantSubscraptionId = subId,
				Amount = subscription.Price,
				ReferenceCode = refCode,
				PaymentDate = DateTime.Now,
				IsPaid = false,
				StartDate = startDate,
				EndDate = endDate,
				IsActive = false
			};

			_context.ApplicantTransactions.Add(transaction);
			_context.SaveChanges();

			TempData["RefCode"] = refCode;
			TempData["Amount"] = subscription.Price.ToString();
			TempData["ApplicantId"] = applicantId;

			return RedirectToAction("PaymentInstructions");
		}

		public IActionResult PaymentInstructions()
		{
			var refCode = TempData["RefCode"]?.ToString();
			var amountStr = TempData["Amount"]?.ToString();
			var applicantId = TempData["ApplicantId"]?.ToString();

			if (string.IsNullOrEmpty(refCode) || string.IsNullOrEmpty(amountStr))
				return RedirectToAction("Index", "Home");

			if (!decimal.TryParse(amountStr, out decimal amount))
				return RedirectToAction("Index", "Home");

			ViewBag.RefCode = refCode;
			ViewBag.Amount = amount;
			ViewBag.ApplicantId = applicantId;

			return View();
		}
	}
}
