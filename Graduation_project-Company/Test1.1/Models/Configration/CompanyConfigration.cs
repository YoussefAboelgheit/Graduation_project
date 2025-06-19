using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test1._1.Models.Entity;

namespace Test1._1.Models.Configration
{
	public class CompanyConfigration : IEntityTypeConfiguration<Company>
	{

		public void Configure(EntityTypeBuilder<Company> builder)
		{
			builder.Property(x => x.TaxCard)
				.HasColumnType("VARCHAR")
				.HasMaxLength(1000)
				.IsRequired();

			builder.Property(x => x.CommercialRegister)
				.HasColumnType("VARCHAR")
				.HasMaxLength(1000)
				.IsRequired();

			builder.Property(x => x.Logo)
				.HasColumnType("VARCHAR")
				.HasMaxLength(1000)
				.IsRequired();

			builder.Property(x => x.CurrentNumEmployees)
				.IsRequired();

			builder.HasMany(x => x.JobAdvertisments)
				   .WithOne(x => x.Company)
				   .HasForeignKey(x => x.CompanyId)
				   .IsRequired();

			builder.Property(x => x.Description)
				.HasColumnType("VARCHAR")
				.HasMaxLength(1000);

			builder.Property(x => x.status)
				   .HasColumnType("VARCHAR")
					.HasMaxLength(20)
					.HasDefaultValue("Pending");

			builder.ToTable("Companies");
        }
	}
}