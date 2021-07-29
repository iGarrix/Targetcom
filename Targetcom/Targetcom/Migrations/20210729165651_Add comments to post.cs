using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Targetcom.Migrations
{
    public partial class Addcommentstopost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfilePostageComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfileCommentatorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PostageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePostageComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfilePostageComments_AspNetUsers_ProfileCommentatorId",
                        column: x => x.ProfileCommentatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfilePostageComments_ProfilePostages_PostageId",
                        column: x => x.PostageId,
                        principalTable: "ProfilePostages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePostageComments_PostageId",
                table: "ProfilePostageComments",
                column: "PostageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePostageComments_ProfileCommentatorId",
                table: "ProfilePostageComments",
                column: "ProfileCommentatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfilePostageComments");
        }
    }
}
