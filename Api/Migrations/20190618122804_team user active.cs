using Microsoft.EntityFrameworkCore.Migrations;

namespace FirmaBudowlana.Migrations
{
    public partial class teamuseractive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Workers",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Teams",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Teams");
        }
    }
}
