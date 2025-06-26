// Controllers/TransactionController.cs
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

			ViewBag.CompanyId = companyId;
			ViewBag.SubscriptionId = subId;
			ViewBag.SubType = sub.SubType;
			ViewBag.Amount = sub.Price;

			return View("PaymentInstructions");
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
				EndDate = DateTime.Now.AddMonths(1),
				IsActive = false
			};

			_context.CompanyTransactions.Add(transaction);
			_context.SaveChanges();

			ViewBag.RefCode = refCode;
			ViewBag.Amount = sub.Price;
			ViewBag.CompanyId = companyId;
			ViewBag.SubType = sub.SubType;

			TempData["Message"] = "Payment code has been generated. Please complete your payment.";
			return View("PaymentInstructions");
		}
	}
}
