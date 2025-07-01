using System.Collections.Generic;
using Test1._1.Models.Entity;

namespace Test1._1.Models.ViewModels
{
	public class AdminDashboardViewModel
	{
		public List<CompanyViewModel> Companies { get; set; } 
		public List<ApplicantSubscraption> ApplicantSubscraptions { get; set; }
		public List<CompanySubscraption> CompanySubscraptions { get; set; }
		public List<CompanyTransaction> PendingCompanyTransactions { get; set; }
		public List<ApplicantTransaction> PendingApplicantTransactions { get; set; }
        public List<Company> PendingCompanies { get; set; }
        public List<EditAdvertisment> PendingEdits { get; set; }
    }
}
