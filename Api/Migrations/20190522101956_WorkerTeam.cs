using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FirmaBudowlana.Migrations
{
    public partial class WorkerTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    TeamID = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamID);
                });

            migrationBuilder.CreateTable(
                name: "Worker",
                columns: table => new
                {
                    WorkerID = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Specialization = table.Column<string>(nullable: true),
                    ManHour = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worker", x => x.WorkerID);
                });

            migrationBuilder.CreateTable(
                name: "WorkerTeam",
                columns: table => new
                {
                    WorkerID = table.Column<Guid>(nullable: false),
                    TeamID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerTeam", x => new { x.WorkerID, x.TeamID });
                    table.ForeignKey(
                        name: "FK_WorkerTeam_Teams_TeamID",
                        column: x => x.TeamID,
                        principalTable: "Team",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerTeam_Workers_WorkerID",
                        column: x => x.WorkerID,
                        principalTable: "Worker",
                        principalColumn: "WorkerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkerTeam_TeamID",
                table: "WorkerTeam",
                column: "TeamID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkerTeam");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Worker");
        }
    }
}
