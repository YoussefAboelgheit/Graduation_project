namespace Test1._1.Models.Entity
{
	public class Payment
	{
		public int Id { get; set; }
		public DateTime PaymentDate { get; set; }
		public decimal Amount { get; set; }
		public bool IsPaid { get; set; }
	}
}