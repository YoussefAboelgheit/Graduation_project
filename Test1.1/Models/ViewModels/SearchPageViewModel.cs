using Test1._1.Models.Entity;
using System.Collections.Generic;
namespace Test1._1.Models.ViewModels
{
	public class SearchPageViewModel
	{
		
		public List<ApplicantCardHomeViewModel> Applicants { get; set; }
		public List<CompanyViewModel> Companies { get; set; }
		public List<CompanyAdvHomeViewModel> Ads { get; set; }
	}
}
//public List<Applicant> Applicants { get; set; }
//public List<Company> Companies { get; set; }
//public List<JobAdvertisment> Advertisements { get; set; }