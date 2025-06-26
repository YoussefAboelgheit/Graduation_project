using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test1._1.Models.Entity;

namespace Test1._1.Models.Configration
{
	public class ApplicantTransactionConfigration : IEntityTypeConfiguration<ApplicantTransaction>
	{
		public void Configure(EntityTypeBuilder<ApplicantTransaction> builder)
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

			builder.HasOne(x => x.Applicant)
				   .WithMany(a => a.ApplicantTranactions)
				   .HasForeignKey(x => x.ApplicantId)
				   .OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(x => x.ApplicantSubscraption)
				   .WithMany(s => s.ApplicantTransactions)
				   .HasForeignKey(x => x.ApplicantSubscraptionId)
				   .OnDelete(DeleteBehavior.NoAction);
		}
	}
}
