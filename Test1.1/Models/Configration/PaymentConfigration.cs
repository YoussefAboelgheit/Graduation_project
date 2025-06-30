using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test1._1.Models.Entity;

namespace Test1._1.Models.Configration
{
	public class PaymentConfigration :  IEntityTypeConfiguration<Payment>
	{
		public void Configure(EntityTypeBuilder<Payment> builder)
		{

			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).UseIdentityColumn();

			builder.Property(x => x.PaymentDate)
				.HasColumnType("DATETIME")
				.IsRequired();

			builder.Property(x => x.Amount)
				.HasPrecision(18, 2)
				.IsRequired();

			builder.ToTable("Payments");

		}
		
	}
}