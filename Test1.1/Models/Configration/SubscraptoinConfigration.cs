using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using test1._0.model;

namespace Test1._1.Models.Configration
{
	public class SubscraptionConfigration : IEntityTypeConfiguration<Subscraption>
	{
		public void Configure(EntityTypeBuilder<Subscraption> builder)
		{
			builder.HasKey(x => x.Sub_id);
			builder.Property(x => x.Sub_id).ValueGeneratedNever();

			builder.Property(x => x.Sub_type)
				.HasColumnType("VARCHAR")
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(x => x.Num_allowed)
				.HasMaxLength(50)
				.IsRequired();
			builder.Property(x => x.Price)
				.HasPrecision(15, 2)
				.IsRequired();


			builder.ToTable("Subscraptions");
		}
	}
}