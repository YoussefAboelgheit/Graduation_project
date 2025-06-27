using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test1._1.Migrations
{
    /// <inheritdoc />
    public partial class addtest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscraptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumAllowed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscraptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fname = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    Lname = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    HashedPassword = table.Column<string>(type: "VARCHAR(60)", maxLength: 60, nullable: false),
                    Phone = table.Column<string>(type: "VARCHAR(75)", maxLength: 75, nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantPayments_Payments_Id",
                        column: x => x.Id,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyPayments_Payments_Id",
                        column: x => x.Id,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantSubscrabtions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantSubscrabtions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantSubscrabtions_Subscraptions_Id",
                        column: x => x.Id,
                        principalTable: "Subscraptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanySubscraptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySubscraptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanySubscraptions_Subscraptions_Id",
                        column: x => x.Id,
                        principalTable: "Subscraptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admins_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Field_work = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    Years_experience = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    CV = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applicants_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Logo = table.Column<string>(type: "VARCHAR(1000)", maxLength: 1000, nullable: false),
                    CurrentNumEmployees = table.Column<int>(type: "int", nullable: false),
                    FiledWork = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxCard = table.Column<string>(type: "VARCHAR(1000)", maxLength: 1000, nullable: false),
                    CommercialRegister = table.Column<string>(type: "VARCHAR(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    AppSubscrabtionId = table.Column<int>(type: "int", nullable: false),
                    ApplicantSubscrabtionId = table.Column<int>(type: "int", nullable: false),
                    AppPaymentId = table.Column<int>(type: "int", nullable: false),
                    ApplicantPaymentId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantTransactions_ApplicantPayments_ApplicantPaymentId",
                        column: x => x.ApplicantPaymentId,
                        principalTable: "ApplicantPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantTransactions_ApplicantSubscrabtions_ApplicantSubscrabtionId",
                        column: x => x.ApplicantSubscrabtionId,
                        principalTable: "ApplicantSubscrabtions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantTransactions_Applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CompanySubId = table.Column<int>(type: "int", nullable: false),
                    CompanySubscraptionId = table.Column<int>(type: "int", nullable: false),
                    CompanyPaymentId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyTransactions_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyTransactions_CompanyPayments_CompanyPaymentId",
                        column: x => x.CompanyPaymentId,
                        principalTable: "CompanyPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyTransactions_CompanySubscraptions_CompanySubscraptionId",
                        column: x => x.CompanySubscraptionId,
                        principalTable: "CompanySubscraptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobAdvertisments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jobdetail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumEmployee = table.Column<int>(type: "int", nullable: false),
                    jobtitle = table.Column<string>(type: "VARCHAR(25)", maxLength: 25, nullable: false),
                    Job_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    governorate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    salary = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: false),
                    language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Certificate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobAdvertisments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobAdvertisments_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantAdvertisments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    JobAdvertismentId = table.Column<int>(type: "int", nullable: false),
                    TellAboutYou = table.Column<string>(type: "Varchar", nullable: false),
                    Submation_Date = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantAdvertisments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantAdvertisments_Applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantAdvertisments_JobAdvertisments_JobAdvertismentId",
                        column: x => x.JobAdvertismentId,
                        principalTable: "JobAdvertisments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantAdvertisments_ApplicantId",
                table: "ApplicantAdvertisments",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantAdvertisments_JobAdvertismentId",
                table: "ApplicantAdvertisments",
                column: "JobAdvertismentId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantTransactions_ApplicantId",
                table: "ApplicantTransactions",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantTransactions_ApplicantPaymentId",
                table: "ApplicantTransactions",
                column: "ApplicantPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantTransactions_ApplicantSubscrabtionId",
                table: "ApplicantTransactions",
                column: "ApplicantSubscrabtionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTransactions_CompanyId",
                table: "CompanyTransactions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTransactions_CompanyPaymentId",
                table: "CompanyTransactions",
                column: "CompanyPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTransactions_CompanySubscraptionId",
                table: "CompanyTransactions",
                column: "CompanySubscraptionId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisments_CompanyId",
                table: "JobAdvertisments",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "ApplicantAdvertisments");

            migrationBuilder.DropTable(
                name: "ApplicantTransactions");

            migrationBuilder.DropTable(
                name: "CompanyTransactions");

            migrationBuilder.DropTable(
                name: "JobAdvertisments");

            migrationBuilder.DropTable(
                name: "ApplicantPayments");

            migrationBuilder.DropTable(
                name: "ApplicantSubscrabtions");

            migrationBuilder.DropTable(
                name: "Applicants");

            migrationBuilder.DropTable(
                name: "CompanyPayments");

            migrationBuilder.DropTable(
                name: "CompanySubscraptions");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Subscraptions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
