using Microsoft.AspNetCore.Mvc;
using Test1._1.Models;
using Test1._1.Models.Entity;
using System;
using System.Linq;

namespace Test1._1.Controllers
{
	public class ApplicantTransactionController : Controller
	{
		private readonly AppDBContext _context;

		public ApplicantTransactionController(AppDBContext context)
		{
			_context = context;
		}

		// Step 1: When user clicks "Subscribe", go to confirmation page
		[HttpPost]
		public IActionResult Select(int subId, string applicantId)
		{
			if (string.IsNullOrEmpty(applicantId) || subId <= 0)
				return RedirectToAction("Index", "Home");

			var sub = _context.ApplicantSubscraptions.FirstOrDefault(s => s.Id == subId);
			if (sub == null)
				return RedirectToAction("Index", "ApplicantSubscrption");

			return RedirectToAction("PaymentInstructions", new
			{
				applicantId = applicantId,
				subscriptionId = subId
			});
		}

		// Step 2: Show confirmation page before generating code
		[HttpGet]
		public IActionResult PaymentInstructions(string applicantId, int subscriptionId, string refCode = null)
		{
			var sub = _context.ApplicantSubscraptions.FirstOrDefault(s => s.Id == subscriptionId);
			if (sub == null || string.IsNullOrEmpty(applicantId))
				return RedirectToAction("Index", "Home");

			ViewBag.ApplicantId = applicantId;
			ViewBag.SubscriptionId = subscriptionId;
			ViewBag.SubType = sub.SubType;
			ViewBag.Amount = sub.Price;
			ViewBag.RefCode = refCode;

			return View();
		}

		// Step 3: Generate code and show updated page
		[HttpPost]
		public IActionResult GenerateCode(string applicantId, int subscriptionId)
		{
			var sub = _context.ApplicantSubscraptions.FirstOrDefault(s => s.Id == subscriptionId);
			if (sub == null) return RedirectToAction("Index", "ApplicantSubscrption");

			string refCode = "APP-" + new Random().Next(1000, 9999);

			var transaction = new ApplicantTransaction
			{
				ApplicantId = applicantId,
				ApplicantSubscraptionId = subscriptionId,
				Amount = sub.Price,
				ReferenceCode = refCode,
				PaymentDate = DateTime.Now,
				IsPaid = false,
				StartDate = DateTime.Now,
				EndDate = CalculateEndDate(sub.SubType),
				IsActive = false
			};

			_context.ApplicantTransactions.Add(transaction);
			_context.SaveChanges();

			return RedirectToAction("PaymentInstructions", new
			{
				applicantId = applicantId,
				subscriptionId = subscriptionId,
				refCode = refCode
			});
		}

		private DateTime CalculateEndDate(string subType)
		{
			switch (subType.ToLower())
			{
				case "daily": return DateTime.Now.AddDays(1);
				case "weekly": return DateTime.Now.AddDays(7);
				case "monthly": return DateTime.Now.AddMonths(1);
				case "yearly": return DateTime.Now.AddYears(1);
				default: return DateTime.Now.AddDays(7); // fallback
			}
		}
	}
}
