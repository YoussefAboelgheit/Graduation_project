
namespace Test1._1.Models.Entity
{
	public class ApplicantTransaction
	{
		public int Id { get; set; }
		public string ApplicantId { get; set; }
		public Applicant Applicant{ get; set; }
		public int ApplicantSubscraptionId { get; set; }
		public ApplicantSubscraption ApplicantSubscraption { get; set; }
		public DateTime PaymentDate { get; set; }
		public decimal Amount { get; set; }
		public string ReferenceCode { get; set; }
		public bool IsPaid { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public bool IsActive { get; set; }
	}
}

