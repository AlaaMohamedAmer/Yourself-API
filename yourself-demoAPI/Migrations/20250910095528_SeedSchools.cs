using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace yourself_demoAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedSchools : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "identity",
                table: "Schools",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "ENG001", "Engineering School" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));
        }
    }
}
