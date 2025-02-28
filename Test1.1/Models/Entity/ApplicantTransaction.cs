

namespace Test1._1.Models.Entity
{
	public class ApplicantTransaction
	{
		public int Id { get; set; }
		public int ApplicantId { get; set; }
		public Applicant Applicant { get; set; }
		public int AppSubscrabtionId { get; set; }
		public ApplicantSubscrabtion ApplicantSubscrabtion { get; set; }
		public int AppPaymentId { get; set; }
		public ApplicantPayment ApplicantPayment { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public bool IsActive { get; set; }
	}
}

