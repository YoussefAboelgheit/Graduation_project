namespace test1._0.model
{
	public class Job_Adv
	{
		public int Adv_Id { get; set; }
		public string Job_detail { get; set; }
		public int Num_Employee { get; set; }
		public string job_title { get; set; }
		public TimeOnly Job_time { get; set; }

		public string governorate { get; set; }
		public decimal salary { get; set; }

		public string language { get; set; }

		public string Certificate { get; set; }

		public int Company_id { get; set; } 
        public Company Companies { get; set; }

		public ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
		public ICollection<Appli_Job_Adv> Appli_job_advs { get; set; } = new List<Appli_Job_Adv>();



	}
}
