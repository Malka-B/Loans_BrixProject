using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rules.Data.Migrations
{
    public partial class rules_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rule",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentRuleId = table.Column<int>(nullable: true),
                    LoanProviderId = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Parameter = table.Column<string>(nullable: true),
                    Operator = table.Column<string>(nullable: true),
                    ValueToCompeare = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rule_Rule_ParentRuleId",
                        column: x => x.ParentRuleId,
                        principalTable: "Rule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rulesParameters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RuleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rulesParameters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rule_ParentRuleId",
                table: "Rule",
                column: "ParentRuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rule");

            migrationBuilder.DropTable(
                name: "rulesParameters");
        }
    }
}
