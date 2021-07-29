using Microsoft.EntityFrameworkCore.Migrations;

namespace Targetcom.Migrations
{
    public partial class Updatedprofilepostage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WritterId",
                table: "ProfilePostages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePostages_WritterId",
                table: "ProfilePostages",
                column: "WritterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilePostages_AspNetUsers_WritterId",
                table: "ProfilePostages",
                column: "WritterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfilePostages_AspNetUsers_WritterId",
                table: "ProfilePostages");

            migrationBuilder.DropIndex(
                name: "IX_ProfilePostages_WritterId",
                table: "ProfilePostages");

            migrationBuilder.DropColumn(
                name: "WritterId",
                table: "ProfilePostages");
        }
    }
}
