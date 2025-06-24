namespace Test1._1.Models.Entity
{
    public class JobAdvertisment
    {
        public int Id { get; set; }
        public string Jobdetail { get; set; }
        public int NumEmployee { get; set; }
        public string jobtitle { get; set; }
        public string Job_time { get; set; }
        public string governorate { get; set; }
        public string salary { get; set; } // Changed from decimal to string to store ranges like "2000-4000"
        public string JobRequirements { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public string CompanyId { get; set; }
        public Company Company { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}