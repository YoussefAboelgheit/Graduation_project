using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test1._1.Models.Entity;

namespace Test1._1.Models.Configration
{
	public class CompanyTransactionConfigration : IEntityTypeConfiguration<CompanyTransaction>
	{
		public void Configure(EntityTypeBuilder<CompanyTransaction> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).UseIdentityColumn();

			builder.Property(x => x.PaymentDate)
				.HasColumnType("DATETIME")
				.IsRequired();

			builder.Property(x => x.StartDate)
				.HasColumnType("DATETIME")
				.IsRequired();

			builder.Property(x => x.EndDate)
				.HasColumnType("DATETIME")
				.IsRequired();

			builder.Property(x => x.Amount)
				.HasPrecision(18, 2)
				.IsRequired();

			builder.Property(x => x.ReferenceCode)
				.HasColumnType("VARCHAR")
				.HasMaxLength(1000)
				.IsRequired();

			builder.HasOne(x => x.Company)
				   .WithMany(x => x.CompanyTransactions)
				   .HasForeignKey(x => x.CompanyId)
				   .OnDelete(DeleteBehavior.NoAction);

			// العلاقة مع ApplicantSubscrabtion
			builder.HasOne(x => x.CompanySubscraption)
				   .WithMany(s => s.CompanyTransactions)
				   .HasForeignKey(x => x.CompanySubscraptionId)
				   .OnDelete(DeleteBehavior.NoAction);
		}
	}
}