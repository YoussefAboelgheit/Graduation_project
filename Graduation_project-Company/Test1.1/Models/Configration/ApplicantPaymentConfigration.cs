using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography.Xml;

using Test1._1.Models.Entity;

namespace Test1._1.Models.Configration
{
	public class ApplicantPaymentConfigration : IEntityTypeConfiguration<ApplicantPayment>
	{
		
			public void Configure(EntityTypeBuilder<ApplicantPayment> builder)
			{

				

				builder.ToTable("Payments");

			}
		}
	}

