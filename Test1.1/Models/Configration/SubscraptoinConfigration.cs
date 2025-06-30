using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test1._1.Models.Entity;

namespace Test1._1.Models.Configration
{
	public class SubscraptionConfigration : IEntityTypeConfiguration<Subscraption>
	{
		public void Configure(EntityTypeBuilder<Subscraption> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).UseIdentityColumn();

			builder.Property(x => x.SubType)
				.HasColumnType("VARCHAR")
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(x => x.NumAllowed)
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(x => x.Price)
				.HasPrecision(18, 2)
				.IsRequired();

			builder.Property("SubscraptionType").HasColumnType("Varchar")
				.HasMaxLength(3);

			builder.ToTable("Subscraptions");
		}
	}
}