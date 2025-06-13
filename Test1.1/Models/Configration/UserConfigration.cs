//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Test1._1.Models.Entity;

//namespace Test1._1.Models.Configration
//{
//	public class UserConfigration : IEntityTypeConfiguration<User>
//	{
//		public void Configure(EntityTypeBuilder<User> builder)
//		{
//			builder.HasKey(x => x.Id);
//			builder.Property(x => x.Id).UseIdentityColumn();

//			builder.Property(x => x.Fname)
//				.HasColumnType("VARCHAR")
//				.HasMaxLength(20)
//				.IsRequired();

//			builder.Property(x => x.Lname)
//				.HasColumnType("VARCHAR")
//				.HasMaxLength(20)
//				.IsRequired();

//			builder.Property(x => x.Phone)
//				.HasColumnType("VARCHAR")
//				.HasMaxLength(75)
//				.IsRequired();

//			builder.Property(x => x.Email)
//				.HasColumnType("VARCHAR")
//				.HasMaxLength(50)
//				.IsRequired();

//			builder.Property(x => x.HashedPassword)
//				.HasColumnType("VARCHAR")
//				.HasMaxLength(60)
//				.IsRequired();
//			builder.Property(x => x.IsDeleted)
//				.HasDefaultValue(false);

//			builder.HasIndex(x => x.Email).IsUnique();

//			builder.HasIndex(x => x.Phone).IsUnique();

//			builder.UseTptMappingStrategy();
//			builder.ToTable("Users");

//			// 🔒 تحقق إن الإيميل لازم ينتهي بـ @gmail.com
//			builder.ToTable("Users", t =>
//			{
//				t.HasCheckConstraint("CK_Users_Email_GmailOnly", "[Email] LIKE '%@gmail.com'");
//			});
//		}
//	}
//}
 