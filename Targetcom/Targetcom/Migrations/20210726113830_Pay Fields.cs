using Microsoft.EntityFrameworkCore.Migrations;

namespace Targetcom.Migrations
{
    public partial class PayFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPremium",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetCoins",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPremium",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TargetCoins",
                table: "AspNetUsers");
        }
    }
}
