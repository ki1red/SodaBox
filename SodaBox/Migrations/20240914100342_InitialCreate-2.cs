using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SodaBox.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 3,
                column: "brandId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 4,
                column: "brandId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 5,
                column: "brandId",
                value: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 3,
                column: "brandId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 4,
                column: "brandId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 5,
                column: "brandId",
                value: 2);
        }
    }
}
