namespace Test1._1.Models.Entity
{
	public class ApplicantAdvertisment
	{
		//not completed yet
		public int Id { get; set; }
		public string ApplicantId { get; set; }
		public Applicant Applicant { get; set; }
		public int JobAdvertismentId { get; set; }
		public JobAdvertisment JobAdvertisment { get; set; }
		public string? TellAboutYou { get; set; }
		public DateTime Submation_Date { get; set; }
		public bool IsDeleted { get; set; }

	}
}