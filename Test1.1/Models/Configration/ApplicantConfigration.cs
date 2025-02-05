using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using test1._0.model;

namespace Test1._1.Models.Configration
{
	public class ApplicantConfigration : IEntityTypeConfiguration<Applicant>
	{
		public void Configure(EntityTypeBuilder<Applicant> builder)
		{
			builder.HasKey(x => x.ApplicantId);
			builder.Property(x => x.ApplicantId).ValueGeneratedNever();

			builder.Property(x => x.Field_work)
				.HasColumnType("VARCHAR")
				.HasMaxLength(20)
				.IsRequired();

			builder.Property(x => x.Years_experience)
				.HasMaxLength(20)
				.IsRequired();


			builder.Property(x => x.CV)
				.HasMaxLength(1500)
				.IsRequired();

			builder.ToTable("Applicant");
		}
		public void Configure(EntityTypeBuilder<Transaction> builder)
		{
		

			builder.Property(x => x.tran_date)
				.HasColumnType("DATETIME")
				.IsRequired();

			builder.Property(x => x.amount)
				.HasPrecision(15,2)
				.IsRequired();
			builder.ToTable("Transactions");
		}



	}
}