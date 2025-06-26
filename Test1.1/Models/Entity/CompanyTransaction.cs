using Stripe;

namespace Test1._1.Models.Entity
{
	public class CompanyTransaction
	{
		public int Id { get; set; }

		public string CompanyId { get; set; }
		public Company Company { get; set; }

		public int CompanySubscraptionId { get; set; }
		public CompanySubscraption CompanySubscraption { get; set; }

		public DateTime PaymentDate { get; set; }
		public decimal Amount { get; set; }
		public string ReferenceCode { get; set; }
		public bool IsPaid { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public bool IsActive { get; set; }
		
	}
}
