using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SodaBox.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    { 1, 1, "images/drinks/coca_cola.png", "Coca Cola", 1.99m, 10 },
                    { 2, 2, "images/drinks/pepsi.png", "Pepsi", 1.89m, 5 },
                    { 3, 2, "images/drinks/coca_cola_zero.png", "Coca Cola Zero", 2.19m, 5 },
                    { 4, 2, "images/drinks/fanta.png", "Fanta", 1.55m, 5 },
                    { 5, 2, "images/drinks/sprite.png", "Sprite", 1.89m, 5 }
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
