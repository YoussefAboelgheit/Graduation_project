
namespace Test1._1.Models.Entity
{
	public class ApplicantSubscrabtion : Subscraption
	{
		public ICollection<ApplicantTransaction> ApplicantTrasactions { get; set; } = new List<ApplicantTransaction>();
	}
}
