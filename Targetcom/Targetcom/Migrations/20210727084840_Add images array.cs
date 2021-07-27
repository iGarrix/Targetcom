using Microsoft.EntityFrameworkCore.Migrations;

namespace Targetcom.Migrations
{
    public partial class Addimagesarray : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UrlImage",
                table: "AspNetUsers",
                newName: "UrlImages");

            migrationBuilder.AddColumn<string>(
                name: "UrlAvatar",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlAvatar",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UrlImages",
                table: "AspNetUsers",
                newName: "UrlImage");
        }
    }
}
