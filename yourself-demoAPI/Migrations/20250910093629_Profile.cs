using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace yourself_demoAPI.Migrations
{
    /// <inheritdoc />
    public partial class Profile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                schema: "identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolId",
                schema: "identity",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Schools",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_SchoolId",
                schema: "identity",
                table: "Users",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Schools_SchoolId",
                schema: "identity",
                table: "Users",
                column: "SchoolId",
                principalSchema: "identity",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Schools_SchoolId",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Schools",
                schema: "identity");

            migrationBuilder.DropIndex(
                name: "IX_Users_SchoolId",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                schema: "identity",
                table: "Users");
        }
    }
}
