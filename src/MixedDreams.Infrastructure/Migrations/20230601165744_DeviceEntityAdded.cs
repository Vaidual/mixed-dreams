using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixedDreams.Application.Migrations
{
    /// <inheritdoc />
    public partial class DeviceEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeviceId",
                table: "BusinessLocations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateDeleted = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessLocations_DeviceId",
                table: "BusinessLocations",
                column: "DeviceId",
                unique: true,
                filter: "[DeviceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CompanyId",
                table: "Devices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Identifier",
                table: "Devices",
                column: "Identifier",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessLocations_Devices_DeviceId",
                table: "BusinessLocations",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessLocations_Devices_DeviceId",
                table: "BusinessLocations");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_BusinessLocations_DeviceId",
                table: "BusinessLocations");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "BusinessLocations");
        }
    }
}
