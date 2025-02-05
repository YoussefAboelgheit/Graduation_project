using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using test1._0.model;

namespace Test1._1.Models.Configration
{
	public class ApplicantConfigration : TransactionConfigration, IEntityTypeConfiguration<Applicant>
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

			builder.HasMany(c => c.Job_Advs)
				.WithMany(x => x.Applicants)
				.UsingEntity<Appli_Job_Adv>();

			builder.ToTable("Applicants");
		}
		
	}
}