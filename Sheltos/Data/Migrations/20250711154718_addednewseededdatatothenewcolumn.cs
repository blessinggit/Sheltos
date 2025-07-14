using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sheltos.Data.Migrations
{
    /// <inheritdoc />
    public partial class addednewseededdatatothenewcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Agents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Agents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "AgentId",
                keyValue: 1,
                columns: new[] { "DateOfBirth", "Gender" },
                values: new object[] { new DateTime(2002, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male" });

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "AgentId",
                keyValue: 2,
                columns: new[] { "DateOfBirth", "Gender" },
                values: new object[] { new DateTime(2002, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2025, 7, 11, 15, 47, 17, 550, DateTimeKind.Utc).AddTicks(5847));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2025, 7, 11, 15, 47, 17, 550, DateTimeKind.Utc).AddTicks(5854));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2025, 7, 11, 15, 47, 17, 550, DateTimeKind.Utc).AddTicks(5917));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Agents");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2025, 7, 11, 13, 55, 1, 3, DateTimeKind.Utc).AddTicks(6207));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2025, 7, 11, 13, 55, 1, 3, DateTimeKind.Utc).AddTicks(6210));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2025, 7, 11, 13, 55, 1, 3, DateTimeKind.Utc).AddTicks(6213));
        }
    }
}
