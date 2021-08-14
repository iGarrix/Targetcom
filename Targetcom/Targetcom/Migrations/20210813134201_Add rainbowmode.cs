using Microsoft.EntityFrameworkCore.Migrations;

namespace Targetcom.Migrations
{
    public partial class Addrainbowmode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InRanbowName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "RainbowMode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RainbowMode",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "InRanbowName",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }
    }
}
