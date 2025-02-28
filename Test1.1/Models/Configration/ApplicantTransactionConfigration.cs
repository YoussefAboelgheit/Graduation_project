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

			// العلاقة مع Applicant
			builder.HasOne(x => x.Applicant)
				   .WithMany(a => a.ApplicantTranactions)
				   .HasForeignKey(x => x.ApplicantId)
				   .OnDelete(DeleteBehavior.Cascade);

			// العلاقة مع ApplicantSubscrabtion
			builder.HasOne(x => x.ApplicantSubscrabtion)
				   .WithMany(s => s.ApplicantTrasactions)
				   .HasForeignKey(x => x.AppSubscrabtionId)
				   .OnDelete(DeleteBehavior.Cascade);

			// العلاقة مع ApplicantPayment
			builder.HasOne(x => x.ApplicantPayment)
				   .WithMany(p => p.ApplicantTransactions)
				   .HasForeignKey(x => x.AppPaymentId)
				   .OnDelete(DeleteBehavior.Cascade);

		}
	}
}
