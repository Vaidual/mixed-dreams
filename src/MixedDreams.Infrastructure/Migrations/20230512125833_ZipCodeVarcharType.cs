using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixedDreams.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ZipCodeVarcharType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address_ZipCode",
                table: "Companies",
                type: "varchar(12)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(12)");

            migrationBuilder.AlterColumn<string>(
                name: "Address_ZipCode",
                table: "BusinessLocations",
                type: "varchar(12)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(12)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address_ZipCode",
                table: "Companies",
                type: "char(12)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(12)");

            migrationBuilder.AlterColumn<string>(
                name: "Address_ZipCode",
                table: "BusinessLocations",
                type: "char(12)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(12)");
        }
    }
}
