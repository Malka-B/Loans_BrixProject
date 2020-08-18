using Microsoft.EntityFrameworkCore.Migrations;

namespace Rules.Data.Migrations
{
    public partial class rules_changeRulesParameter1_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_rulesParameters",
                table: "rulesParameters");

            migrationBuilder.RenameTable(
                name: "rulesParameters",
                newName: "RulesParameters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RulesParameters",
                table: "RulesParameters",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RulesParameters",
                table: "RulesParameters");

            migrationBuilder.RenameTable(
                name: "RulesParameters",
                newName: "rulesParameters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rulesParameters",
                table: "rulesParameters",
                column: "Id");
        }
    }
}
