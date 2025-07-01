namespace Test1._1.Models.Entity
{
	public class ApplicantSubscraption
	{
		public int Id { get; set; }
		public string SubType { get; set; }
		public decimal Price { get; set; }
		public ICollection<ApplicantTransaction> ApplicantTransactions { get; set; } = new List<ApplicantTransaction>();
	}
}
