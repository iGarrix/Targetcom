using Microsoft.EntityFrameworkCore.Migrations;

namespace Targetcom.Migrations
{
    public partial class deleteforkeygames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_ProfileId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_ProfileId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Games");

            migrationBuilder.CreateTable(
                name: "GameProfile",
                columns: table => new
                {
                    GamesId = table.Column<int>(type: "int", nullable: false),
                    ProfilesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameProfile", x => new { x.GamesId, x.ProfilesId });
                    table.ForeignKey(
                        name: "FK_GameProfile_AspNetUsers_ProfilesId",
                        column: x => x.ProfilesId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameProfile_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameProfile_ProfilesId",
                table: "GameProfile",
                column: "ProfilesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameProfile");

            migrationBuilder.AddColumn<string>(
                name: "ProfileId",
                table: "Games",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_ProfileId",
                table: "Games",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_ProfileId",
                table: "Games",
                column: "ProfileId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
