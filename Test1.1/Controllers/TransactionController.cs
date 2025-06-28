using Microsoft.AspNetCore.Mvc;
using Test1._1.Models;
using Test1._1.Models.Entity;
using System;
using System.Linq;

namespace Test1._1.Controllers
{
	public class TransactionController : Controller
	{
		private readonly AppDBContext _context;

		public TransactionController(AppDBContext context)
		{
			_context = context;
		}

		[HttpPost]
		public IActionResult Select(int subId, string companyId)
		{
			if (string.IsNullOrEmpty(companyId) || subId <= 0)
				return RedirectToAction("Index", "Home");

			var sub = _context.CompanySubscraptions.FirstOrDefault(s => s.Id == subId);
			if (sub == null) return RedirectToAction("Index", "CompanySubscraptions");

			// إعادة التوجيه للعرض بطريقة آمنة
			return RedirectToAction("PaymentInstructions", new
			{
				companyId = companyId,
				subscriptionId = subId
			});
		}

		[HttpGet]
		public IActionResult PaymentInstructions(string companyId, int subscriptionId, string refCode = null)
		{
			var sub = _context.CompanySubscraptions.FirstOrDefault(s => s.Id == subscriptionId);
			if (sub == null || string.IsNullOrEmpty(companyId))
				return RedirectToAction("Index", "Home");

			ViewBag.CompanyId = companyId;
			ViewBag.SubscriptionId = subscriptionId;
			ViewBag.SubType = sub.SubType;
			ViewBag.Amount = sub.Price;
			ViewBag.RefCode = refCode; // null لو أول مرة

			return View();
		}

		[HttpPost]
		public IActionResult GenerateCode(string companyId, int subscriptionId)
		{
			var sub = _context.CompanySubscraptions.FirstOrDefault(s => s.Id == subscriptionId);
			if (sub == null) return RedirectToAction("Index", "CompanySubscraptions");

			string refCode = "JOB-" + new Random().Next(1000, 9999);

			var transaction = new CompanyTransaction
			{
				CompanyId = companyId,
				CompanySubscraptionId = subscriptionId,
				Amount = sub.Price,
				ReferenceCode = refCode,
				PaymentDate = DateTime.Now,
				IsPaid = false,
				StartDate = DateTime.Now,
				EndDate = CalculateEndDate(sub.SubType),
				IsActive = false
			};

			_context.CompanyTransactions.Add(transaction);
			_context.SaveChanges();

			// إعادة التوجيه باستخدام query string لتفادي إعادة إرسال البيانات
			return RedirectToAction("PaymentInstructions", new
			{
				companyId = companyId,
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
				default: return DateTime.Now.AddMonths(1); // fallback
			}
		}
	}
}
