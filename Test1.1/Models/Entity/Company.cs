namespace Test1._1.Models.Entity
{
    public class Company : ApplicationUser
    {

        public string Logo { get; set; }
        public int CurrentNumEmployees { get; set; }
        public string FiledWork { get; set; }
        public string TaxCard { get; set; }
        public string CommercialRegister { get; set; }
        public string Description { get; set; }
        public string status { get; set; } = "Pending";
        public ICollection<JobAdvertisment> JobAdvertisments { get; set; } = new List<JobAdvertisment>();

        public ICollection<CompanyTransaction> CompanyTransactions { get; set; } = new List<CompanyTransaction>();

    }
}
