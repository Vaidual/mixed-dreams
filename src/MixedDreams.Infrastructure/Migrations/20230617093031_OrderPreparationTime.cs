using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixedDreams.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderPreparationTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "PreparationTime",
                table: "Products",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "CooksNumber",
                table: "Companies",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreparationTime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CooksNumber",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "AspNetUsers");
        }
    }
}
