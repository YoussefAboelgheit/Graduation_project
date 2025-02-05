using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Runtime.Intrinsics.Arm;

namespace test1._0.model
{
	public class AppDBContext:DbContext
	{


		public virtual DbSet<Applicant> Applicant { get; set; } 
		public virtual DbSet<A_Subscrabtion> A_Subscrabtion { get; set; } 
		public virtual DbSet<A_Transaction> A_Transaction { get; set; } 
		public virtual DbSet<Admin> Admin { get; set; } 
		public virtual DbSet<Appli_job_adv> Appli_job_adv { get; set; } 
		public virtual DbSet<C_Subscraption> C_Subscraption { get; set; } 
		public virtual DbSet<C_Transaction> C_Transaction { get; set; } 
		public virtual DbSet<Com_Sub_Taction> Com_Sub_Taction { get; set; } 
		public virtual DbSet<Company> Company { get; set; } 
		public virtual DbSet<Job_Adv> Job_Adv { get; set; } 
		public virtual DbSet<Subscraption> Subscraption { get; set; } 

		public virtual DbSet<Transaction> Transaction { get; set; } 
		public virtual DbSet<User> User { get; set; } = null!;




		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
			    .Build();

			var connectionstring = config.GetSection("constr").Value;
			optionsBuilder.UseSqlServer(connectionstring);
		}
	}
}
