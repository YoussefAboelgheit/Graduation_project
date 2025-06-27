using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test1._1.Migrations
{
    /// <inheritdoc />
    public partial class FixCompanyTransactionFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyPayments");

            migrationBuilder.DropColumn(
                name: "CompanySubsId",
                table: "CompanyTransactions");

            migrationBuilder.DropColumn(
                name: "AppSubsId",
                table: "ApplicantTransactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanySubsId",
                table: "CompanyTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AppSubsId",
                table: "ApplicantTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CompanyPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferenceCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPayments", x => x.Id);
                });
        }
    }
}
