
namespace Test1._1.Models.Entity
{
	public class CompanySubscrabtion : Subscraption
	{
	
		public ICollection<CompanyTransaction> CompanyTransactions { get; set; } = new List<CompanyTransaction>();
	}
}
