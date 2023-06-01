using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixedDreams.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BusinessLocationInOrdersAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BusinessLocationId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BusinessLocationId",
                table: "Orders",
                column: "BusinessLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_BusinessLocations_BusinessLocationId",
                table: "Orders",
                column: "BusinessLocationId",
                principalTable: "BusinessLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_BusinessLocations_BusinessLocationId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BusinessLocationId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BusinessLocationId",
                table: "Orders");
        }
    }
}
