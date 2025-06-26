
namespace Test1._1.Models.Entity
{
    public class JobAdvertisment
    {
        public int Id { get; set; }
        public string Jobdetail { get; set; }
        public int NumEmployee { get; set; }
        public string jobtitle { get; set; }
        public TimeOnly Job_time { get; set; }
        public string governorate { get; set; }
        public decimal salary { get; set; }
        public string language { get; set; }
        public string Certificate { get; set; }

        public string CompanyId { get; set; }
        public Company Company { get; set; }

        //public ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
