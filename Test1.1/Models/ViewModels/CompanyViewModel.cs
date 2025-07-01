namespace Test1._1.Models.ViewModels
{
	public class CompanyViewModel
	{
		public string Id { get; set; }  // from ApplicationUser
		public string UserName { get; set; }  // from ApplicationUser
		public string Email { get; set; }     // from ApplicationUser
		public string PhoneNumber { get; set; } // from ApplicationUser

		public string Logo { get; set; }
        public string FiledWork { get; set; }
		public int CurrentNumEmployees { get; set; }
        public string Description { get; set; }
		public string TaxCard { get; set; }
		public string CommercialRegister { get; set; }
		public string Status { get; set; }

		public List<CompanyAdvHomeViewModel> JobAdvertisements { get; set; } = new List<CompanyAdvHomeViewModel>();
	}
}
