using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sheltos.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedseededdatatonewcolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NinNo",
                table: "Agents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Qualifications",
                table: "Agents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "AgentId",
                keyValue: 1,
                columns: new[] { "NinNo", "Qualifications" },
                values: new object[] { "423578983873", "M.sc Holder" });

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "AgentId",
                keyValue: 2,
                columns: new[] { "NinNo", "Qualifications" },
                values: new object[] { "423578983873", "O`Level Holder" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NinNo",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "Qualifications",
                table: "Agents");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2025, 7, 10, 10, 5, 45, 44, DateTimeKind.Utc).AddTicks(2308));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2025, 7, 10, 10, 5, 45, 44, DateTimeKind.Utc).AddTicks(2315));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2025, 7, 10, 10, 5, 45, 44, DateTimeKind.Utc).AddTicks(2318));
        }
    }
}
