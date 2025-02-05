namespace test1._0.model
{
	public class Applicant:User
	{
		public int ApplicantId { get; set; }
		public string Field_work { get; set; }
		public int Years_experience { get; set; }
		public string CV { get; set; }
		public ICollection<Job_Adv>Job_Advs { get; set; } = new List<Job_Adv>();
		public ICollection<Appli_Job_Adv> Appli_job_advs { get; set; } = new List<Appli_Job_Adv>();





	}
}
