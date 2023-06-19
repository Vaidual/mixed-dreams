using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixedDreams.Application.Migrations
{
    /// <inheritdoc />
    public partial class CompanyAcceptOrdersAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreparationStarted",
                table: "Orders");

            migrationBuilder.AddColumn<bool>(
                name: "AcceptOrders",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptOrders",
                table: "Companies");

            migrationBuilder.AddColumn<bool>(
                name: "PreparationStarted",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
