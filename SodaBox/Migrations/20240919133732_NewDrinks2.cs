using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SodaBox.Migrations
{
    /// <inheritdoc />
    public partial class NewDrinks2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "drinks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "imagePath",
                table: "drinks",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "brands",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
                values: new object[] { 5, "Dr Pepper" });

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

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "brandId", "imagePath", "name", "price", "quantity" },
                values: new object[] { 1, "images/drinks/coca_cola_zero.png", "Coca Cola Zero", 70, 0 });

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "brandId", "imagePath", "name", "price", "quantity" },
                values: new object[] { 2, "images/drinks/pepsi.png", "Pepsi", 40, 3 });

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "brandId", "imagePath", "name", "price", "quantity" },
                values: new object[] { 2, "images/drinks/pepsi_zero.png", "Pepsi Zero", 45, 3 });

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "brandId", "imagePath", "name", "quantity" },
                values: new object[] { 3, "images/drinks/fanta.png", "Fanta", 2 });

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "brandId", "imagePath", "name", "price", "quantity" },
                values: new object[] { 4, "images/drinks/sprite.png", "Sprite", 55, 0 });

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "brandId", "imagePath", "name", "price" },
                values: new object[] { 5, "images/drinks/dr_pepper.png", "Dr Pepper", 65 });

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "brandId", "imagePath", "name", "price", "quantity" },
                values: new object[] { 5, "images/drinks/dr_pepper_cherry.png", "Dr Pepper Cherry", 40, 1 });

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
                name: "orderItems");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DeleteData(
                table: "brands",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "drinks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "imagePath",
                table: "drinks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "brands",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "brandId", "imagePath", "name", "price", "quantity" },
                values: new object[] { 2, "images/drinks/pepsi.png", "Pepsi", 40, 3 });

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "brandId", "imagePath", "name", "price", "quantity" },
                values: new object[] { 1, "images/drinks/coca_cola_zero.png", "Coca Cola Zero", 70, 0 });

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "brandId", "imagePath", "name", "price", "quantity" },
                values: new object[] { 3, "images/drinks/fanta.png", "Fanta", 55, 2 });

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "brandId", "imagePath", "name", "quantity" },
                values: new object[] { 4, "images/drinks/sprite.png", "Sprite", 0 });

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "brandId", "imagePath", "name", "price", "quantity" },
                values: new object[] { 1, "images/drinks/coca_cola.png", "Coca Cola", 65, 1 });

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "brandId", "imagePath", "name", "price" },
                values: new object[] { 2, "images/drinks/pepsi.png", "Pepsi", 40 });

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "brandId", "imagePath", "name", "price", "quantity" },
                values: new object[] { 4, "images/drinks/sprite.png", "Sprite", 55, 2 });
        }
    }
}
