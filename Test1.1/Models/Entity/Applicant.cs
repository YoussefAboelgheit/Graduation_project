
namespace Test1._1.Models.Entity
{
	public class Applicant:ApplicationUser
	{
        //
        public string lastName { get; set; }

		public string Field_work { get; set; }
		public int Years_experience { get; set; }
		public string CV { get; set; }
        public string Profile_image { get; set; }
		public ICollection<ApplicantAdvertisment> ApplicantAdvertisments { get; set; } = new List<ApplicantAdvertisment>();
		
		public ICollection<ApplicantTransaction> ApplicantTranactions { get; set; } = new List<ApplicantTransaction>();
	}
}
