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
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "drinks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    brandId = table.Column<int>(type: "int", nullable: false),
                    imagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.InsertData(
                table: "brands",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Coca Cola" },
                    { 2, "Pepsi" },
                    { 3, "Fanta" },
                    { 4, "Sprite" }
                });

            migrationBuilder.InsertData(
                table: "drinks",
                columns: new[] { "id", "brandId", "imagePath", "name", "price", "quantity" },
                values: new object[,]
                {
                    { 1, 1, "images/drinks/coca_cola.png", "Coca Cola", 65, 3 },
                    { 2, 2, "images/drinks/pepsi.png", "Pepsi", 40, 3 },
                    { 3, 1, "images/drinks/coca_cola_zero.png", "Coca Cola Zero", 70, 0 },
                    { 4, 3, "images/drinks/fanta.png", "Fanta", 55, 2 },
                    { 5, 4, "images/drinks/sprite.png", "Sprite", 55, 0 },
                    { 6, 1, "images/drinks/coca_cola.png", "Coca Cola", 65, 1 },
                    { 7, 2, "images/drinks/pepsi.png", "Pepsi", 40, 1 },
                    { 8, 4, "images/drinks/sprite.png", "Sprite", 55, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_drinks_brandId",
                table: "drinks",
                column: "brandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "drinks");

            migrationBuilder.DropTable(
                name: "brands");
        }
    }
}
