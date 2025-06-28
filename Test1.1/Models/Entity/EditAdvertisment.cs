namespace Test1._1.Models.Entity
{
    public class EditAdvertisment
    {
        public int Id { get; set; }
        public DateTime EditDate { get; set; } = DateTime.Now;
        public string EditorId { get; set; } // User who made the edit

        // Fields from JobAdvertisment that can be edited
        public string JobTitle { get; set; }
        public string JobDetail { get; set; }
        public string JobTime { get; set; }
        public string Governorate { get; set; }
        public string Salary { get; set; }
        public string JobRequirements { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        // Reference to the original JobAdvertisment
        public int JobAdvertismentId { get; set; }
        public JobAdvertisment JobAdvertisment { get; set; }
    }
}