using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FirmaBudowlana.Migrations
{
    public partial class OrderPaid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderID",
                table: "Payments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "Orders",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Paid",
                table: "Orders");

        }
    }
}
