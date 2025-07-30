using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sheltos.Data.Migrations
{
    /// <inheritdoc />
    public partial class addadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Properties",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AdminId", "DateTime" },
                values: new object[] { null, new DateTime(2025, 7, 21, 14, 50, 31, 620, DateTimeKind.Utc).AddTicks(8524) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AdminId", "DateTime" },
                values: new object[] { null, new DateTime(2025, 7, 21, 14, 50, 31, 620, DateTimeKind.Utc).AddTicks(8529) });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AdminId", "DateTime" },
                values: new object[] { null, new DateTime(2025, 7, 21, 14, 50, 31, 620, DateTimeKind.Utc).AddTicks(8532) });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_AdminId",
                table: "Properties",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_AspNetUsers_AdminId",
                table: "Properties",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_AspNetUsers_AdminId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_AdminId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Properties");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2025, 7, 19, 21, 32, 46, 2, DateTimeKind.Utc).AddTicks(1350));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2025, 7, 19, 21, 32, 46, 2, DateTimeKind.Utc).AddTicks(1354));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2025, 7, 19, 21, 32, 46, 2, DateTimeKind.Utc).AddTicks(1356));
        }
    }
}
