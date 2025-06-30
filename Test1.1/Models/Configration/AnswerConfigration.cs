using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test1._1.Models.Entity;

namespace Test1._1.Models.Configration
{
    public class AnswerConfigration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Response)
                .HasColumnType("Varchar")
                .HasMaxLength(1500)
                .IsRequired();

            builder.HasOne(x => x.ApplicantAdvertisments)
                   .WithMany(x => x.Answers)
                   .HasForeignKey(x => x.ApplicantAdvertismentId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Questions)
                   .WithMany(x => x.Answers)
                   .HasForeignKey(x => x.QuestionId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("Answers");

        }
    }
}