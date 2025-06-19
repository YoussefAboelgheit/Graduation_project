using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test1._1.Models.Entity;

namespace Test1._1.Models.Configration
{
	public class QuestionConfigration : IEntityTypeConfiguration<Question>
	{
		public void Configure(EntityTypeBuilder<Question> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).UseIdentityColumn();

			builder.Property(x => x.Text)
				.HasColumnType("Varchar")
				.HasMaxLength(250)
				.IsRequired();

			builder.Property(x => x.Type)
				.HasColumnType("Varchar")
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(x => x.IsShared)
				   .HasColumnType("bit")
				   .IsRequired();

			builder.HasOne(x => x.JobAdvertisment)
				   .WithMany(x => x.Questions)
				   .HasForeignKey(x => x.JobAdvertismentId)
				   .OnDelete(DeleteBehavior.NoAction);  

			builder.ToTable("Questions");

		}
	}
}