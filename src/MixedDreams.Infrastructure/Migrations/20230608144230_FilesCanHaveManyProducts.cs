using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixedDreams.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FilesCanHaveManyProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BackblazeFiles_Products_ProductId",
                table: "BackblazeFiles");

            migrationBuilder.DropIndex(
                name: "IX_BackblazeFiles_ProductId",
                table: "BackblazeFiles");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "BackblazeFiles");

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "Products",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ImageId",
                table: "Products",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_BackblazeFiles_ImageId",
                table: "Products",
                column: "ImageId",
                principalTable: "BackblazeFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_BackblazeFiles_ImageId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ImageId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "BackblazeFiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BackblazeFiles_ProductId",
                table: "BackblazeFiles",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BackblazeFiles_Products_ProductId",
                table: "BackblazeFiles",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
