using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MixedDreams.Application.Migrations
{
    /// <inheritdoc />
    public partial class ProductIngredientAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientProduct");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4904100c-4446-4dfd-b236-df8e86fa072f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "577e8ce8-ee5e-4662-8381-1078abaa6e3a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2775bb6-bb77-461a-ac5f-0b01b6cc8614");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "HasAmount",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Ingredients");

            migrationBuilder.CreateTable(
                name: "ProductIngredient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IngredientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HasAmount = table.Column<bool>(type: "bit", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: true),
                    Unit = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductIngredient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductIngredient_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductIngredient_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0ee54ee3-101d-4b34-84eb-bfef315a1dd3", null, "Administator", "ADMINISTATOR" },
                    { "690c3db8-3d4b-4cbb-b115-f33098e00d8a", null, "Company", "COMPANY" },
                    { "e37f6baf-f42d-4596-abab-653b5e2dc124", null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductIngredient_IngredientId",
                table: "ProductIngredient",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductIngredient_ProductId",
                table: "ProductIngredient",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductIngredient");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0ee54ee3-101d-4b34-84eb-bfef315a1dd3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "690c3db8-3d4b-4cbb-b115-f33098e00d8a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e37f6baf-f42d-4596-abab-653b5e2dc124");

            migrationBuilder.AddColumn<float>(
                name: "Amount",
                table: "Ingredients",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAmount",
                table: "Ingredients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "Ingredients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IngredientProduct",
                columns: table => new
                {
                    IngredientsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientProduct", x => new { x.IngredientsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_IngredientProduct_Ingredients_IngredientsId",
                        column: x => x.IngredientsId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredientProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4904100c-4446-4dfd-b236-df8e86fa072f", null, "Company", "COMPANY" },
                    { "577e8ce8-ee5e-4662-8381-1078abaa6e3a", null, "Customer", "CUSTOMER" },
                    { "e2775bb6-bb77-461a-ac5f-0b01b6cc8614", null, "Administator", "ADMINISTATOR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientProduct_ProductsId",
                table: "IngredientProduct",
                column: "ProductsId");
        }
    }
}
