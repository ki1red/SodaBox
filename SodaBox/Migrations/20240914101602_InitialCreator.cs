using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SodaBox.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "price",
                table: "drinks",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 1,
                column: "price",
                value: 100);

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 2,
                column: "price",
                value: 95);

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 3,
                column: "price",
                value: 130);

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 4,
                column: "price",
                value: 120);

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 5,
                column: "price",
                value: 120);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "drinks",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 1,
                column: "price",
                value: 1.99m);

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 2,
                column: "price",
                value: 1.89m);

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 3,
                column: "price",
                value: 2.19m);

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 4,
                column: "price",
                value: 1.55m);

            migrationBuilder.UpdateData(
                table: "drinks",
                keyColumn: "id",
                keyValue: 5,
                column: "price",
                value: 1.89m);
        }
    }
}
