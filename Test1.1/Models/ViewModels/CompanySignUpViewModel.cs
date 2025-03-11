namespace Test1._1.Models.ViewModels
{
    public class CompanySignUpViewModel
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public IFormFile Logo { get; set; } // For file upload
        public string FiledWork { get; set; }
        public string TaxCard { get; set; }
        public string CommercialRegister { get; set; }
        public string Description { get; set; }
    }
}