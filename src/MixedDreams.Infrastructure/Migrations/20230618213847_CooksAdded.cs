using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixedDreams.Application.Migrations
{
    /// <inheritdoc />
    public partial class CooksAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PreparationStarted",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "NextProductInQueueId",
                table: "OrderProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentProductOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastProductOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentEndTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastEndTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cooks_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cooks_OrderProducts_CurrentProductOrderId",
                        column: x => x.CurrentProductOrderId,
                        principalTable: "OrderProducts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cooks_OrderProducts_LastProductOrderId",
                        column: x => x.LastProductOrderId,
                        principalTable: "OrderProducts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_NextProductInQueueId",
                table: "OrderProducts",
                column: "NextProductInQueueId");

            migrationBuilder.CreateIndex(
                name: "IX_Cooks_CompanyId",
                table: "Cooks",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Cooks_CurrentProductOrderId",
                table: "Cooks",
                column: "CurrentProductOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Cooks_LastProductOrderId",
                table: "Cooks",
                column: "LastProductOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_OrderProducts_NextProductInQueueId",
                table: "OrderProducts",
                column: "NextProductInQueueId",
                principalTable: "OrderProducts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_OrderProducts_NextProductInQueueId",
                table: "OrderProducts");

            migrationBuilder.DropTable(
                name: "Cooks");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_NextProductInQueueId",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "PreparationStarted",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "NextProductInQueueId",
                table: "OrderProducts");
        }
    }
}
