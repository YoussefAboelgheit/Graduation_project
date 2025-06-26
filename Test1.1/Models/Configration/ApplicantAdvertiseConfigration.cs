using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test1._1.Models.Entity;

namespace Test1._1.Models.Configration
{
    public class ApplicantAdvertiseConfigration : IEntityTypeConfiguration<ApplicantAdvertisment>
    {
        public void Configure(EntityTypeBuilder<ApplicantAdvertisment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();


            builder.Property(x => x.SubmissionDate)
                .HasColumnType("DATETIME")
                .IsRequired();


            builder.HasOne(x => x.Applicant)
                   .WithMany(x => x.ApplicantAdvertisments)
                   .HasForeignKey(x => x.ApplicantId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("ApplicantAdvertisments");

        }
    }
}