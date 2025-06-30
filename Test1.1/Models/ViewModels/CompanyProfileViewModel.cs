using Test1._1.Models.Entity;

namespace Test1._1.Models.ViewModels
{
    public class CompanyProfileViewModel
    {
        public Company Company { get; set; }

        public List<JobAdvertisment> JobAdvertisments { get; set; }

        public List<ApplicantCardHomeViewModel> SuggestedApplicants { get; set; }
    }
}
