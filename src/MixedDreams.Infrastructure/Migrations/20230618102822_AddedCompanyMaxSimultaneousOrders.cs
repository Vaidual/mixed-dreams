using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixedDreams.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompanyMaxSimultaneousOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "MaxSimultaneousOrders",
                table: "Companies",
                type: "smallint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxSimultaneousOrders",
                table: "Companies");
        }
    }
}
