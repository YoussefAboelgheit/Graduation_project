using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test1._1.Models.Entity;

namespace Test1._1.Models.Configration
{
	public class ApplicantConfigration :  IEntityTypeConfiguration<Applicant>
	{
		public void Configure(EntityTypeBuilder<Applicant> builder)
		{
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
			

			
		}
	}
}