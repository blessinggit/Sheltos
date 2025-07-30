using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sheltos.Data.Migrations
{
    /// <inheritdoc />
    public partial class updaterequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropertyId",
                table: "PropertyRequests",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "PropertyRequests");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2025, 7, 18, 17, 42, 52, 219, DateTimeKind.Utc).AddTicks(8714));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2025, 7, 18, 17, 42, 52, 219, DateTimeKind.Utc).AddTicks(8720));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2025, 7, 18, 17, 42, 52, 219, DateTimeKind.Utc).AddTicks(8723));
        }
    }
}
