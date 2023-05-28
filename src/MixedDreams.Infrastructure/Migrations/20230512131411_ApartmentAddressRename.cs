using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixedDreams.Application.Migrations
{
    /// <inheritdoc />
    public partial class ApartmentAddressRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address_Apartament",
                table: "Companies",
                newName: "Address_Apartment");

            migrationBuilder.RenameColumn(
                name: "Address_Apartament",
                table: "BusinessLocations",
                newName: "Address_Apartment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address_Apartment",
                table: "Companies",
                newName: "Address_Apartament");

            migrationBuilder.RenameColumn(
                name: "Address_Apartment",
                table: "BusinessLocations",
                newName: "Address_Apartament");
        }
    }
}
