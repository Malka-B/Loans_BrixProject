using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Loans.Data.Migrations
{
    public partial class loans_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    LoanProviderID = table.Column<Guid>(nullable: false),
                    LoanDate = table.Column<DateTime>(nullable: false),
                    LoanStatus = table.Column<int>(nullable: false),
                    BorrowerID = table.Column<int>(nullable: false),
                    Salary = table.Column<int>(nullable: false),
                    FixedSalary = table.Column<bool>(nullable: false),
                    Birthdate = table.Column<DateTime>(nullable: false),
                    LoanAmount = table.Column<int>(nullable: false),
                    SeniorityYearsInBank = table.Column<int>(nullable: false),
                    NumberOfRepayments = table.Column<int>(nullable: false),
                    LoanPurpose = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FailureRules",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanId = table.Column<Guid>(nullable: false),
                    RuleId = table.Column<int>(nullable: false),
                    RuleDescription = table.Column<string>(nullable: true),
                    ManagerComment = table.Column<string>(nullable: true),
                    ManagerSignature = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailureRules", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FailureRules_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FailureRules_LoanId",
                table: "FailureRules",
                column: "LoanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FailureRules");

            migrationBuilder.DropTable(
                name: "Loans");
        }
    }
}
