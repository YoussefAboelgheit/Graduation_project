using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test1._1.Models.Entity;

namespace Test1._1.Models.Configration
{
    public class ApplicantAdvertiseConfigration : IEntityTypeConfiguration<ApplicantAdvertisment>
    {
        public void Configure(EntityTypeBuilder<ApplicantAdvertisment> builder)
        {
            builder.HasKey(aa => aa.Id);
            builder.Property(aa => aa.Id).UseIdentityColumn();

            builder.Property(aa => aa.SubmissionDate)
                .HasColumnType("DATETIME")
                .IsRequired();

            builder.Property(aa => aa.Status)
                .HasColumnType("VARCHAR")
                .HasMaxLength(20)
                .IsRequired();

            // Configure relationships
            builder.HasOne(aa => aa.Applicant)
                .WithMany(a => a.ApplicantAdvertisments)
                .HasForeignKey(aa => aa.ApplicantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(aa => aa.JobAdvertisment)
                .WithMany(ja => ja.Applications)
                .HasForeignKey(aa => aa.JobAdvertismentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("ApplicantAdvertisements");
        }
    }
}