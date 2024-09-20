using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SodaBox.Migrations
{
    /// <inheritdoc />
    public partial class NewDrinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "brands",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "coins",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    price = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    imagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coins", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    totalAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "drinks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    brandId = table.Column<int>(type: "int", nullable: false),
                    imagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drinks", x => x.id);
                    table.ForeignKey(
                        name: "FK_drinks_brands_brandId",
                        column: x => x.brandId,
                        principalTable: "brands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orderItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    brandName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    drinkName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    Orderid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_orderItems_orders_Orderid",
                        column: x => x.Orderid,
                        principalTable: "orders",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "brands",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Coca Cola" },
                    { 2, "Pepsi" },
                    { 3, "Fanta" },
                    { 4, "Sprite" },
                    { 5, "Dr Pepper" }
                });

            migrationBuilder.InsertData(
                table: "coins",
                columns: new[] { "id", "imagePath", "price", "quantity" },
                values: new object[,]
                {
                    { 1, "images/coins/1.png", 1, 20 },
                    { 2, "images/coins/2.png", 2, 20 },
                    { 3, "images/coins/5.png", 5, 20 },
                    { 4, "images/coins/10.png", 10, 20 }
                });

            migrationBuilder.InsertData(
                table: "drinks",
                columns: new[] { "id", "brandId", "imagePath", "name", "price", "quantity" },
                values: new object[,]
                {
                    { 1, 1, "images/drinks/coca_cola.png", "Coca Cola", 65, 3 },
                    { 2, 1, "images/drinks/coca_cola_zero.png", "Coca Cola Zero", 70, 0 },
                    { 3, 2, "images/drinks/pepsi.png", "Pepsi", 40, 3 },
                    { 4, 2, "images/drinks/pepsi_zero.png", "Pepsi Zero", 45, 3 },
                    { 5, 3, "images/drinks/fanta.png", "Fanta", 55, 2 },
                    { 6, 4, "images/drinks/sprite.png", "Sprite", 55, 0 },
                    { 7, 5, "images/drinks/dr_pepper.png", "Dr Pepper", 65, 1 },
                    { 8, 5, "images/drinks/dr_pepper_cherry.png", "Dr Pepper Cherry", 40, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_drinks_brandId",
                table: "drinks",
                column: "brandId");

            migrationBuilder.CreateIndex(
                name: "IX_orderItems_Orderid",
                table: "orderItems",
                column: "Orderid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "coins");

            migrationBuilder.DropTable(
                name: "drinks");

            migrationBuilder.DropTable(
                name: "orderItems");

            migrationBuilder.DropTable(
                name: "brands");

            migrationBuilder.DropTable(
                name: "orders");
        }
    }
}
