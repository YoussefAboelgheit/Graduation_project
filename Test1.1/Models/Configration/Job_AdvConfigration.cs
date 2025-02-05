using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using test1._0.model;

namespace Test1._1.Models.Configration
{
	public class Job_AdvConfigration
	{
		public void Configure(EntityTypeBuilder<Job_Adv> builder)
		{

			builder.HasKey(x => x.Adv_Id);
			builder.Property(x => x.Adv_Id).ValueGeneratedNever();


			builder.Property(x => x.Num_Employee)
				.HasMaxLength(1000)
				.IsRequired();

			builder.Property(x => x.salary)
				.HasPrecision(15, 2)
				.IsRequired();

			builder.Property(x => x.Job_time)
				.HasColumnType("TIME")
				.IsRequired();

			builder.Property(x => x.job_title)
				.HasColumnType("VARCHAR")
				.HasMaxLength(25)
				.IsRequired();

			builder.Property(x => x.Certificate)
				.HasMaxLength(10000)
				.IsRequired();

			builder.Property(x => x.governorate)
				.HasColumnType("VARCHAR")
				.HasMaxLength(25)
				.IsRequired();

			builder.Property(x => x.Job_detail)
				.HasColumnType("VARCHAR")
				.HasMaxLength(1000)
				.IsRequired();

			builder.Property(x => x.language)
				.HasColumnType("VARCHAR")
				.HasMaxLength(25)
				.IsRequired();
			builder.HasOne(x => x.Companies)
				.WithMany(x => x.Job_Advs)
				.HasForeignKey(x => x.Company_id)
				.IsRequired();
				
			

			builder.ToTable("Job_Advs");
		}
	}
}