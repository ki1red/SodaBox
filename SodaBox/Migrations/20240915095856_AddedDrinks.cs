using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SodaBox.Migrations
{
    /// <inheritdoc />
    public partial class AddedDrinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "drinks",
                columns: new[] { "id", "brandId", "imagePath", "name", "price", "quantity" },
                values: new object[,]
                {
                    { 6, 4, "images/drinks/sprite.png", "Sprite", 120, 3 },
                    { 7, 4, "images/drinks/sprite.png", "Sprite", 120, 0 },
                    { 8, 3, "images/drinks/fanta.png", "Fanta", 120, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 8);
        }
    }
}
