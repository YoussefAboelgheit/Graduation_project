using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test1._1.Models.Entity;

namespace Test1._1.Models.Configration
{
	public class ComSubConfigration : IEntityTypeConfiguration<CompanySubscraption>
	{
		public void Configure(EntityTypeBuilder<CompanySubscraption> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).UseIdentityColumn();

			builder.Property(x => x.SubType)
				.HasColumnType("VARCHAR")
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(x => x.Price)
				.HasPrecision(18, 2)
				.IsRequired();

			builder.ToTable("AppSubs");
		}
	}
}