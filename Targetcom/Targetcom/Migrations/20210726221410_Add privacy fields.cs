using Microsoft.EntityFrameworkCore.Migrations;

namespace Targetcom.Migrations
{
    public partial class Addprivacyfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShortDate",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Privacy",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VisibilityAboutMe",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VisibilityCommerceData",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VisibilityCommunity",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VisibilityFriends",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VisibilityImages",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VisibilityPlaylist",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VisibilityPostage",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VisibilityQuote",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShortDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Privacy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VisibilityAboutMe",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VisibilityCommerceData",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VisibilityCommunity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VisibilityFriends",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VisibilityImages",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VisibilityPlaylist",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VisibilityPostage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VisibilityQuote",
                table: "AspNetUsers");
        }
    }
}
