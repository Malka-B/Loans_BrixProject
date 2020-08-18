using Microsoft.EntityFrameworkCore.Migrations;

namespace Rules.Data.Migrations
{
    public partial class rules_changeRulesParameter_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RuleName",
                table: "rulesParameters");

            migrationBuilder.AddColumn<string>(
                name: "ParameterName",
                table: "rulesParameters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParameterType",
                table: "rulesParameters",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParameterName",
                table: "rulesParameters");

            migrationBuilder.DropColumn(
                name: "ParameterType",
                table: "rulesParameters");

            migrationBuilder.AddColumn<string>(
                name: "RuleName",
                table: "rulesParameters",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
