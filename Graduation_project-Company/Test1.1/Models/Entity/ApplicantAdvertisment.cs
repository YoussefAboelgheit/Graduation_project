namespace Test1._1.Models.Entity
{
	public class ApplicantAdvertisment
	{
		public int Id { get; set; }
		public string ApplicantId { get; set;}
		public Applicant Applicant { get; set;}
		public DateTime SubmissionDate { get; set;}
		public bool IsDeleted { get; set;}
		public ICollection<Answer> Answers { get; set; } = new List<Answer>();

	}
}