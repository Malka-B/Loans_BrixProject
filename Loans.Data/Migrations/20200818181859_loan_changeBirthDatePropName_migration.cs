using Microsoft.EntityFrameworkCore.Migrations;

namespace Loans.Data.Migrations
{
    public partial class loan_changeBirthDatePropName_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Birthdate",
                table: "Loans",
                newName: "BirthDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "Loans",
                newName: "Birthdate");
        }
    }
}
