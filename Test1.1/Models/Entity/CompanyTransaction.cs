namespace Test1._1.Models.Entity
{
	public class CompanyTransaction
	{
		public int Id { get; set; }
		public int CompanyId { get; set; }
		public Company Company { get; set; }
		public int CompanySubId { get; set; }
		public CompanySubscrabtion CompanySubscraption { get; set; }
		public int CompanyPaymentId { get; set; }
		public CompanyPayment CompanyPayment { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public bool IsActive { get; set; }
	}
}
