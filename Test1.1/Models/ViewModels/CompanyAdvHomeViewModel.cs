namespace Test1._1.Models.ViewModels
{
    public class CompanyAdvHomeViewModel
    {
        public int AdvertisementId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string LogoPath { get; set; }

        // Job Advertisement Details
        public string JobTitle { get; set; }
        public string Salary { get; set; }
        public string Location { get; set; }
        public string JobTime { get; set; }
        public DateTime? CreatedDate { get; set; }

        // Additional properties that might be useful
        public string CompanyId { get; set; }
        public string JobDescription { get; set; }
        public string Requirements { get; set; }
    }
}
