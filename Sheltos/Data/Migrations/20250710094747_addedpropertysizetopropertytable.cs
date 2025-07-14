using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sheltos.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedpropertysizetopropertytable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PropertySize",
                table: "Properties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTime", "PropertySize" },
                values: new object[] { new DateTime(2025, 7, 10, 9, 47, 46, 848, DateTimeKind.Utc).AddTicks(9252), 0.0 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateTime", "PropertySize" },
                values: new object[] { new DateTime(2025, 7, 10, 9, 47, 46, 848, DateTimeKind.Utc).AddTicks(9259), 0.0 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateTime", "PropertySize" },
                values: new object[] { new DateTime(2025, 7, 10, 9, 47, 46, 848, DateTimeKind.Utc).AddTicks(9262), 0.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertySize",
                table: "Properties");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2025, 7, 9, 18, 25, 55, 698, DateTimeKind.Utc).AddTicks(9182));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2025, 7, 9, 18, 25, 55, 698, DateTimeKind.Utc).AddTicks(9186));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2025, 7, 9, 18, 25, 55, 698, DateTimeKind.Utc).AddTicks(9188));
        }
    }
}
