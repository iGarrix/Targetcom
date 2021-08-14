using Microsoft.EntityFrameworkCore.Migrations;

namespace Targetcom.Migrations
{
    public partial class Removesongsandcommunities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisibilityCommunity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VisibilityPlaylist",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VisibilityCommunity",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VisibilityPlaylist",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }
    }
}
