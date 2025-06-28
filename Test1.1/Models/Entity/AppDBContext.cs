using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Test1._1.Models.Configration;

namespace Test1._1.Models.Entity
{
    public class AppDBContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;

        public AppDBContext(DbContextOptions<AppDBContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Subscraption> Subscraptions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<JobAdvertisment> JobAdvertisments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<ApplicantAdvertisment> ApplicantAdvertisments { get; set; }
        public DbSet<ApplicantSubscrabtion> ApplicantSubscrabtions { get; set; }
        public DbSet<ApplicantPayment> ApplicantPayments { get; set; }
        public DbSet<ApplicantTransaction> ApplicantTransactions { get; set; }
        public DbSet<CompanySubscrabtion> CompanySubscraptions { get; set; }
        public DbSet<CompanyPayment> CompanyPayments { get; set; }
        public DbSet<CompanyTransaction> CompanyTransactions { get; set; }

        public DbSet<EditAdvertisment> EditAdvertisments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Subscraption>().UseTptMappingStrategy();
            modelBuilder.Entity<Payment>().UseTptMappingStrategy();

            modelBuilder.ApplyConfiguration(new ApplicantConfigration());
            modelBuilder.ApplyConfiguration(new CompanyConfigration());
            modelBuilder.ApplyConfiguration(new JobAdvConfigration());
            modelBuilder.ApplyConfiguration(new ApplicantAdvertiseConfigration());
            modelBuilder.ApplyConfiguration(new AnswerConfigration());
        }
    }
}