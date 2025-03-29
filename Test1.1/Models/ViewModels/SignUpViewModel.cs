namespace Test1._1.Models.ViewModels
{
    public class SignUpViewModel
    {
        public CompanySignUpViewModel Company { get; set; } = new CompanySignUpViewModel();
        public ApplicantSignUpViewModel Applicant { get; set; } = new ApplicantSignUpViewModel();
    }

}
