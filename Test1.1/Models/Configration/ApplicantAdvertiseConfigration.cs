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

            builder.Property(x => x.JobAdvertismentId)
                .IsRequired();

            // Relationship with Applicant
            builder.HasOne(x => x.Applicant)
                   .WithMany(x => x.ApplicantAdvertisments)
                   .HasForeignKey(x => x.ApplicantId)
                   .OnDelete(DeleteBehavior.NoAction);

            // Relationship with JobAdvertisment
            builder.HasOne(x => x.JobAdvertisment)
                   .WithMany(x => x.Applications)
                   .HasForeignKey(x => x.JobAdvertismentId)
                   .OnDelete(DeleteBehavior.NoAction);

            // Composite unique index to prevent duplicate applications
            builder.HasIndex(x => new { x.ApplicantId, x.JobAdvertismentId })
                   .IsUnique()
                   .HasDatabaseName("IX_ApplicantAdvertisment_Unique");

            builder.ToTable("ApplicantAdvertisments");
        }
    }
}