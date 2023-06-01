using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MixedDreams.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "899de6a5-9859-4311-9903-555f72cc4c37", null, "Company", "COMPANY" },
                    { "98df1cae-35b7-4927-ba8b-7ecf8f2d4e4a", null, "Administator", "ADMINISTATOR" },
                    { "f39d3a5b-37d0-4c4a-a18e-6a5b57514c2a", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "899de6a5-9859-4311-9903-555f72cc4c37");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "98df1cae-35b7-4927-ba8b-7ecf8f2d4e4a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f39d3a5b-37d0-4c4a-a18e-6a5b57514c2a");
        }
    }
}
