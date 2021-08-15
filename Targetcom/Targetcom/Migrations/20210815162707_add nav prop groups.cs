using Microsoft.EntityFrameworkCore.Migrations;

namespace Targetcom.Migrations
{
    public partial class addnavpropgroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FriendId",
                table: "MessageGroups",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageGroups_FriendId",
                table: "MessageGroups",
                column: "FriendId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageGroups_AspNetUsers_FriendId",
                table: "MessageGroups",
                column: "FriendId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageGroups_AspNetUsers_FriendId",
                table: "MessageGroups");

            migrationBuilder.DropIndex(
                name: "IX_MessageGroups_FriendId",
                table: "MessageGroups");

            migrationBuilder.DropColumn(
                name: "FriendId",
                table: "MessageGroups");
        }
    }
}
