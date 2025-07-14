using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sheltos.Data.Migrations
{
    /// <inheritdoc />
    public partial class seededpropertysize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateTime", "PropertySize" },
                values: new object[] { new DateTime(2025, 7, 10, 10, 5, 45, 44, DateTimeKind.Utc).AddTicks(2308), 5000.0 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateTime", "PropertySize" },
                values: new object[] { new DateTime(2025, 7, 10, 10, 5, 45, 44, DateTimeKind.Utc).AddTicks(2315), 4000.0 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateTime", "PropertySize" },
                values: new object[] { new DateTime(2025, 7, 10, 10, 5, 45, 44, DateTimeKind.Utc).AddTicks(2318), 7000.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
