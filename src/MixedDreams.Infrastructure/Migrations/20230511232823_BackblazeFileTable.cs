using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MixedDreams.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BackblazeFileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryImage",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "BackblazeFiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(200)", nullable: false),
                    FileName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Path = table.Column<string>(type: "varchar(200)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackblazeFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackblazeFiles_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BackblazeFiles_ProductId",
                table: "BackblazeFiles",
                column: "ProductId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackblazeFiles");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryImage",
                table: "Products",
                type: "varchar(2100)",
                nullable: true);
        }
    }
}
