using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ItemCardapios",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "Preco",
                value: 5.00m);

            migrationBuilder.UpdateData(
                table: "ItemCardapios",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "Preco",
                value: 4.50m);

            migrationBuilder.UpdateData(
                table: "ItemCardapios",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "Preco",
                value: 7.00m);

            migrationBuilder.UpdateData(
                table: "ItemCardapios",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                column: "Preco",
                value: 2.00m);

            migrationBuilder.UpdateData(
                table: "ItemCardapios",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                column: "Preco",
                value: 2.50m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ItemCardapios",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "Preco",
                value: 5.0);

            migrationBuilder.UpdateData(
                table: "ItemCardapios",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "Preco",
                value: 4.5);

            migrationBuilder.UpdateData(
                table: "ItemCardapios",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "Preco",
                value: 7.0);

            migrationBuilder.UpdateData(
                table: "ItemCardapios",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"),
                column: "Preco",
                value: 2.0);

            migrationBuilder.UpdateData(
                table: "ItemCardapios",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                column: "Preco",
                value: 2.5);
        }
    }
}
