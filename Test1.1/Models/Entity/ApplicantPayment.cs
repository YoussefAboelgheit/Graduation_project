namespace Test1._1.Models.Entity
{
	public class ApplicantPayment : Payment
	{
		public ICollection<ApplicantTransaction> ApplicantTransactions { get; set; } = new List<ApplicantTransaction>();
	}
}