using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixedDreams.Application.Migrations
{
    /// <inheritdoc />
    public partial class TenantIdForProductIngredientAndProductHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "ProductIngredient",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "ProductHistory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ProductIngredient");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ProductHistory");
        }
    }
}
