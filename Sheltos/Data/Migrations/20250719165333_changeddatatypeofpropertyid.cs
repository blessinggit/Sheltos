using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sheltos.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeddatatypeofpropertyid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "PropertyRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2025, 7, 19, 16, 53, 32, 572, DateTimeKind.Utc).AddTicks(9220));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2025, 7, 19, 16, 53, 32, 572, DateTimeKind.Utc).AddTicks(9225));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2025, 7, 19, 16, 53, 32, 572, DateTimeKind.Utc).AddTicks(9227));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PropertyId",
                table: "PropertyRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2025, 7, 18, 18, 14, 45, 334, DateTimeKind.Utc).AddTicks(2334));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2025, 7, 18, 18, 14, 45, 334, DateTimeKind.Utc).AddTicks(2342));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2025, 7, 18, 18, 14, 45, 334, DateTimeKind.Utc).AddTicks(2345));
        }
    }
}
