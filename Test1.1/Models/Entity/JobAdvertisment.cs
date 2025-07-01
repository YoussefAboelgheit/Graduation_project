namespace Test1._1.Models.Entity
{
    public class JobAdvertisment
    {
        public int Id { get; set; }
        public string Jobdetail { get; set; }
        public string jobtitle { get; set; }
        public string Job_time { get; set; }
        public string governorate { get; set; }
        public string salary { get; set; } // Changed from decimal to string to store ranges like "2000-4000"
        public string JobRequirements { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Add field to track if edits are pending
        public bool HasPendingEdits { get; set; } = false;

        // Navigation properties
        public string CompanyId { get; set; }
        public Company Company { get; set; }
        public DateTime ExpiryDate { get; set; } = DateTime.Now.AddMonths(1);
        public bool IsActive { get; set; } = true;
        public bool IsManuallyDeactivated { get; set; } = false;

        public ICollection<EditAdvertisment> EditHistory { get; set; } = new List<EditAdvertisment>();
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<ApplicantAdvertisment> Applications { get; set; } = new List<ApplicantAdvertisment>();
    }
}