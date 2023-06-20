using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixedDreams.Application.Migrations
{
    /// <inheritdoc />
    public partial class ProductPreparationFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPreparations_Cooks_CookId",
                table: "ProductPreparations");

            migrationBuilder.DropIndex(
                name: "IX_ProductPreparations_CookId",
                table: "ProductPreparations");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Cooks_CurrentProductPreparationId",
                table: "Cooks");

            migrationBuilder.DropIndex(
                name: "IX_Cooks_LastProductPreparationId",
                table: "Cooks");

            migrationBuilder.DropColumn(
                name: "CookId",
                table: "ProductPreparations");

            migrationBuilder.CreateIndex(
                name: "IX_Cooks_CurrentProductPreparationId",
                table: "Cooks",
                column: "CurrentProductPreparationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cooks_LastProductPreparationId",
                table: "Cooks",
                column: "LastProductPreparationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cooks_ProductPreparations_CurrentProductPreparationId",
                table: "Cooks",
                column: "CurrentProductPreparationId",
                principalTable: "ProductPreparations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cooks_ProductPreparations_CurrentProductPreparationId",
                table: "Cooks");

            migrationBuilder.DropIndex(
                name: "IX_Cooks_CurrentProductPreparationId",
                table: "Cooks");

            migrationBuilder.DropIndex(
                name: "IX_Cooks_LastProductPreparationId",
                table: "Cooks");

            migrationBuilder.AddColumn<Guid>(
                name: "CookId",
                table: "ProductPreparations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Cooks_CurrentProductPreparationId",
                table: "Cooks",
                column: "CurrentProductPreparationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPreparations_CookId",
                table: "ProductPreparations",
                column: "CookId",
                unique: true,
                filter: "[CookId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cooks_LastProductPreparationId",
                table: "Cooks",
                column: "LastProductPreparationId",
                unique: true,
                filter: "[LastProductPreparationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPreparations_Cooks_CookId",
                table: "ProductPreparations",
                column: "CookId",
                principalTable: "Cooks",
                principalColumn: "CurrentProductPreparationId");
        }
    }
}
