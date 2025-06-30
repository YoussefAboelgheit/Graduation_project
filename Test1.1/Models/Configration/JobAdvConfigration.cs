using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Test1._1.Models.Entity;

namespace Test1._1.Models.Configration
{
	public class JobAdvConfigration : IEntityTypeConfiguration<JobAdvertisment>
	{
		public void Configure(EntityTypeBuilder<JobAdvertisment> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).UseIdentityColumn();

			builder.Property(x => x.jobtitle)
				.HasColumnType("VARCHAR")
				.HasMaxLength(25)
				.IsRequired();

			builder.Property(x => x.salary)
				.HasPrecision(15, 2)
				.IsRequired();



			builder.HasOne(x => x.Company)
				   .WithMany(x => x.JobAdvertisments)
				   .HasForeignKey(x => x.CompanyId)
				   .OnDelete(DeleteBehavior.NoAction);

			

			builder.ToTable("JobAdvertisments");
		}
	}
}
