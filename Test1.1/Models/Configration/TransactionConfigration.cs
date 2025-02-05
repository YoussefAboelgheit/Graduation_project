using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using test1._0.model;

namespace Test1._1.Models.Configration
{
	public class TransactionConfigration : IEntityTypeConfiguration<Transaction>
	{
		public void Configure(EntityTypeBuilder<Transaction> builder)
		{

			builder.Property(x => x.tran_date)
				.HasColumnType("DATETIME")
				.IsRequired();

			builder.Property(x => x.amount)
				.HasPrecision(15, 2)
				.IsRequired();
			builder.ToTable("Transactions");
		}

		


	}
}