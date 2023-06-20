using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixedDreams.Application.Migrations
{
    /// <inheritdoc />
    public partial class ProductPreparationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cooks_OrderProducts_CurrentProductOrderId",
                table: "Cooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Cooks_OrderProducts_LastProductOrderId",
                table: "Cooks");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_OrderProducts_NextProductInQueueId",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_NextProductInQueueId",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_Cooks_CurrentProductOrderId",
                table: "Cooks");

            migrationBuilder.DropIndex(
                name: "IX_Cooks_LastProductOrderId",
                table: "Cooks");

            migrationBuilder.DropColumn(
                name: "NextProductInQueueId",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "CurrentProductOrderId",
                table: "Cooks");

            migrationBuilder.RenameColumn(
                name: "LastProductOrderId",
                table: "Cooks",
                newName: "LastProductPreparationId");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentProductPreparationId",
                table: "Cooks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Cooks_CurrentProductPreparationId",
                table: "Cooks",
                column: "CurrentProductPreparationId");

            migrationBuilder.CreateTable(
                name: "ProductPreparations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CookId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NextProductInQueueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPreparations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPreparations_Cooks_CookId",
                        column: x => x.CookId,
                        principalTable: "Cooks",
                        principalColumn: "CurrentProductPreparationId");
                    table.ForeignKey(
                        name: "FK_ProductPreparations_OrderProducts_OrderProductId",
                        column: x => x.OrderProductId,
                        principalTable: "OrderProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductPreparations_ProductPreparations_NextProductInQueueId",
                        column: x => x.NextProductInQueueId,
                        principalTable: "ProductPreparations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cooks_LastProductPreparationId",
                table: "Cooks",
                column: "LastProductPreparationId",
                unique: true,
                filter: "[LastProductPreparationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPreparations_CookId",
                table: "ProductPreparations",
                column: "CookId",
                unique: true,
                filter: "[CookId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPreparations_NextProductInQueueId",
                table: "ProductPreparations",
                column: "NextProductInQueueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPreparations_OrderProductId",
                table: "ProductPreparations",
                column: "OrderProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cooks_ProductPreparations_LastProductPreparationId",
                table: "Cooks",
                column: "LastProductPreparationId",
                principalTable: "ProductPreparations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cooks_ProductPreparations_LastProductPreparationId",
                table: "Cooks");

            migrationBuilder.DropTable(
                name: "ProductPreparations");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Cooks_CurrentProductPreparationId",
                table: "Cooks");

            migrationBuilder.DropIndex(
                name: "IX_Cooks_LastProductPreparationId",
                table: "Cooks");

            migrationBuilder.DropColumn(
                name: "CurrentProductPreparationId",
                table: "Cooks");

            migrationBuilder.RenameColumn(
                name: "LastProductPreparationId",
                table: "Cooks",
                newName: "LastProductOrderId");

            migrationBuilder.AddColumn<Guid>(
                name: "NextProductInQueueId",
                table: "OrderProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentProductOrderId",
                table: "Cooks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_NextProductInQueueId",
                table: "OrderProducts",
                column: "NextProductInQueueId");

            migrationBuilder.CreateIndex(
                name: "IX_Cooks_CurrentProductOrderId",
                table: "Cooks",
                column: "CurrentProductOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Cooks_LastProductOrderId",
                table: "Cooks",
                column: "LastProductOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cooks_OrderProducts_CurrentProductOrderId",
                table: "Cooks",
                column: "CurrentProductOrderId",
                principalTable: "OrderProducts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cooks_OrderProducts_LastProductOrderId",
                table: "Cooks",
                column: "LastProductOrderId",
                principalTable: "OrderProducts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_OrderProducts_NextProductInQueueId",
                table: "OrderProducts",
                column: "NextProductInQueueId",
                principalTable: "OrderProducts",
                principalColumn: "Id");
        }
    }
}
