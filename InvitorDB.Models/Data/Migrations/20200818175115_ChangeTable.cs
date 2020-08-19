using Microsoft.EntityFrameworkCore.Migrations;

namespace InvitorDB.Models.Migrations
{
    public partial class ChangeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Review",
                table: "EvaluationForms",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Review",
                table: "EvaluationForms");
        }
    }
}
