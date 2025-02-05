using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using test1._0.model;

namespace Test1._1.Models.Configration
{
	public class UserConfigration : CompanyConfigration
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{


			builder.Property(x => x.Fname)
				.HasColumnType("VARCHAR")
				.HasMaxLength(20)
				.IsRequired();

			builder.Property(x => x.Lname)
				.HasColumnType("VARCHAR")
				.HasMaxLength(20)
				.IsRequired();

			builder.Property(x => x.Phone)
				.HasColumnType("VARCHAR")
				.HasMaxLength(75)
				.IsRequired();

			builder.Property(x => x.Email)
				.HasColumnType("VARCHAR")
				.HasMaxLength(50)
				.IsRequired();



			builder.Property(x => x.Email)
				.HasColumnType("VARCHAR")
				.HasMaxLength(50)
				.IsRequired();

			builder.ToTable("Users");
		}
	}
}