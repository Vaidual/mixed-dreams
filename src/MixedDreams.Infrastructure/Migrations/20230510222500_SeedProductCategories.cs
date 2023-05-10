using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MixedDreams.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "ProductCategories");

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "Id", "DateDeleted", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { new Guid("a9572488-e307-4d70-ad4c-64dfe31819a1"), null, false, "Salad" },
                    { new Guid("a9572488-e307-4d70-ad4c-64dfe31819a2"), null, false, "Soup" },
                    { new Guid("a9572488-e307-4d70-ad4c-64dfe31819a3"), null, false, "Snacks" },
                    { new Guid("a9572488-e307-4d70-ad4c-64dfe31819a4"), null, false, "Garnish" },
                    { new Guid("a9572488-e307-4d70-ad4c-64dfe31819a5"), null, false, "Meat" },
                    { new Guid("a9572488-e307-4d70-ad4c-64dfe31819a6"), null, false, "Fish" },
                    { new Guid("a9572488-e307-4d70-ad4c-64dfe31819a7"), null, false, "Dessert" },
                    { new Guid("a9572488-e307-4d70-ad4c-64dfe31819a8"), null, false, "Full meal" },
                    { new Guid("a9572488-e307-4d70-ad4c-64dfe31819a9"), null, false, "Other" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: new Guid("a9572488-e307-4d70-ad4c-64dfe31819a1"));

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: new Guid("a9572488-e307-4d70-ad4c-64dfe31819a2"));

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: new Guid("a9572488-e307-4d70-ad4c-64dfe31819a3"));

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: new Guid("a9572488-e307-4d70-ad4c-64dfe31819a4"));

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: new Guid("a9572488-e307-4d70-ad4c-64dfe31819a5"));

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: new Guid("a9572488-e307-4d70-ad4c-64dfe31819a6"));

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: new Guid("a9572488-e307-4d70-ad4c-64dfe31819a7"));

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: new Guid("a9572488-e307-4d70-ad4c-64dfe31819a8"));

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: new Guid("a9572488-e307-4d70-ad4c-64dfe31819a9"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "ProductCategories",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateUpdated",
                table: "ProductCategories",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
