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

			builder.HasOne(x=>x.Company)
				   .WithMany(x=>x.CompanyTransactions)
				   .HasForeignKey(x => x.CompanyId)
				   .OnDelete(DeleteBehavior.Cascade);


			// العلاقة مع ApplicantSubscrabtion
			builder.HasOne(x => x.CompanySubscraption)
				   .WithMany(s => s.CompanyTransactions)
				   .HasForeignKey(x => x.CompanySubId)
				   .OnDelete(DeleteBehavior.Cascade);

			// العلاقة مع ApplicantPayment
			builder.HasOne(x => x.CompanyPayment)
				   .WithMany(p => p.CompanyTransactions)
				   .HasForeignKey(x => x.CompanyPaymentId)
				   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}