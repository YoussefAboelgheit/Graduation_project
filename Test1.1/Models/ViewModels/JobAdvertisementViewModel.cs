using System.ComponentModel.DataAnnotations;

namespace Test1._1.Models.ViewModels
{
    public class JobAdvertisementViewModel
    {
        [Required]
        public string CompanyId { get; set; }

        [Required]
        [Display(Name = "Field of Work")]
        public string FieldWork { get; set; }

        [Required]
        [Display(Name = "Salary Range")]
        public string Salary { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Job Time")]
        public string JobTime { get; set; }

        [Required]
        [Display(Name = "Job Requirements")]
        public string Job_Requirements { get; set; }

        [Required]
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }

        // For dynamic questions
        public List<string> CustomQuestions { get; set; } = new List<string>();
    }
}