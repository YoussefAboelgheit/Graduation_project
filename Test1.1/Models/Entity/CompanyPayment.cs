namespace Test1._1.Models.Entity
{
	public class CompanyPayment : Payment
	{
		public ICollection<CompanyTransaction> CompanyTransactions { get; set; } = new List<CompanyTransaction>();
	}
}
