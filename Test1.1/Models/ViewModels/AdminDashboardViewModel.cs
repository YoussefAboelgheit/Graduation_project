using Test1._1.Models.Entity;

namespace Test1._1.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<Company> PendingCompanies { get; set; }
        public List<EditAdvertisment> PendingEdits { get; set; }
    }
}
