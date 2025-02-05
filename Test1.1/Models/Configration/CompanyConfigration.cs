using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using test1._0.model;

namespace Test1._1.Models.Configration
{
	public class CompanyConfigration
	{

		public void Configure(EntityTypeBuilder<Company> builder)
		{


			builder.Property(x => x.Tax_card)
				.HasColumnType("VARCHAR")
				.HasMaxLength(1000)
				.IsRequired();

			builder.Property(x => x.Commercia_register)
				.HasColumnType("VARCHAR")
				.HasMaxLength(1000)
				.IsRequired();

			builder.Property(x => x.Logo)
				.HasColumnType("VARCHAR")
				.HasMaxLength(1000)
				.IsRequired();

			builder.Property(x => x.Current_num_employees)
				.HasMaxLength(10000)
				.IsRequired();

			builder.ToTable("Companies");
		}
	}
}