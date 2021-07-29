using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Targetcom.Migrations
{
    public partial class Intropostage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfilePostages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsObject = table.Column<bool>(type: "bit", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNessessaredLikedPost = table.Column<bool>(type: "bit", nullable: false),
                    IsNessessaredSharedPost = table.Column<bool>(type: "bit", nullable: false),
                    IsNessessaredCommentedPost = table.Column<bool>(type: "bit", nullable: false),
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePostages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfilePostages_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LikedProfilePostages",
                columns: table => new
                {
                    ProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProfilePostageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikedProfilePostages", x => new { x.ProfileId, x.ProfilePostageId });
                    table.ForeignKey(
                        name: "FK_LikedProfilePostages_AspNetUsers_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LikedProfilePostages_ProfilePostages_ProfilePostageId",
                        column: x => x.ProfilePostageId,
                        principalTable: "ProfilePostages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikedProfilePostages_ProfilePostageId",
                table: "LikedProfilePostages",
                column: "ProfilePostageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePostages_ProfileId",
                table: "ProfilePostages",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikedProfilePostages");

            migrationBuilder.DropTable(
                name: "ProfilePostages");
        }
    }
}
