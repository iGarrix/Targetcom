using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Targetcom.Migrations
{
    public partial class AddBannedtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BannedId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BannedProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AdminId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPermanent = table.Column<bool>(type: "bit", nullable: false),
                    ReasonDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannedProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannedProfiles_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BannedProfiles_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BannedProfiles_AdminId",
                table: "BannedProfiles",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_BannedProfiles_ProfileId",
                table: "BannedProfiles",
                column: "ProfileId",
                unique: true,
                filter: "[ProfileId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BannedProfiles");

            migrationBuilder.DropColumn(
                name: "BannedId",
                table: "AspNetUsers");
        }
    }
}
