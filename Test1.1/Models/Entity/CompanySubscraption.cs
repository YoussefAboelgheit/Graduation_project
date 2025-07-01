namespace Test1._1.Models.Entity
{
	public class CompanySubscraption
	{
		public int Id { get; set; }
		public string SubType { get; set; }
		public decimal Price { get; set; }
		public ICollection<CompanyTransaction> CompanyTransactions { get; set; } = new List<CompanyTransaction>();
	}
}
