using Test1._1.Models.Entity;

namespace Test1._1.Models.ViewModels
{
	public class AdminDashboardViewModel
	{
		public List<Company> Companies { get; set; } 
		public List<ApplicantSubscraption> ApplicantSubscraptions { get; set; }
		public List<CompanySubscraption> CompanySubscraptions { get; set; }
		public List<CompanyTransaction> PendingCompanyTransactions { get; set; }
		public List<ApplicantTransaction> PendingApplicantTransactions { get; set; }
		
	}
}
