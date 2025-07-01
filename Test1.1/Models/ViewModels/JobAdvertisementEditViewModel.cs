using System.ComponentModel.DataAnnotations;

namespace Test1._1.Models.ViewModels
{
    public class JobAdvertisementEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string FieldWork { get; set; }

        [Required]
        public string JobDescription { get; set; }

        [Required]
        public string JobTime { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Salary { get; set; }

        [Required]
        public string Job_Requirements { get; set; }

        // Add property for pending edits warning
        public bool HasPendingEdits { get; set; }

        public string CompanyId { get; set; }
    }
}