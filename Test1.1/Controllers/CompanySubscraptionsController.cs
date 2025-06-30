using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Test1._1.Models;
using Test1._1.Models.Entity;

namespace Test1._1.Controllers
{
    public class CompanySubscraptionsController : Controller
    {
        private readonly AppDBContext _context;

        public CompanySubscraptionsController(AppDBContext context)
        {
            _context = context;
        }

        // Step 1: عند الضغط على زر Subscribe يتم التوجيه لصفحة التأكيد
        [HttpPost]
        public IActionResult Select(int subId, string companyId)
        {
            if (string.IsNullOrEmpty(companyId) || subId <= 0)
                return RedirectToAction("Index", "Home");

            var sub = _context.CompanySubscraptions.FirstOrDefault(s => s.Id == subId);
            if (sub == null)
                return RedirectToAction("Index", "CompanySubscraptions");

            return RedirectToAction("PaymentInstructions", new
            {
                companyId = companyId,
                subscriptionId = subId
            });
        }

        // Step 2: عرض صفحة التأكيد بدون إنشاء كود
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
            ViewBag.RefCode = refCode;

            return View("~/Views/Transaction/PaymentInstructions.cshtml");
        }

        // Step 3: إنشاء كود الدفع وتخزين المعاملة
        [HttpPost]
        public IActionResult GenerateCode(string companyId, int subscriptionId)
        {
            var sub = _context.CompanySubscraptions.FirstOrDefault(s => s.Id == subscriptionId);
            if (sub == null)
                return RedirectToAction("Index", "CompanySubscraptions");

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
                default: return DateTime.Now.AddMonths(1);
            }
        }
    }
}