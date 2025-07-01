using Test1._1.Models.Entity;

namespace Test1._1.Models.ViewModels
{
    public class ApplicantProfileViewModel
    {
        public Applicant Applicant { get; set; }

        public List<CompanyAdvHomeViewModel> SuggestedAds { get; set; }

        public List<ApplicantAdvertisment> AppliedAdvertisements { get; set; }
    }
}
