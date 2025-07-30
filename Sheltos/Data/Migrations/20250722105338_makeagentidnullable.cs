using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sheltos.Data.Migrations
{
    /// <inheritdoc />
    public partial class makeagentidnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Agents_AgentId",
                table: "Properties");

            migrationBuilder.AlterColumn<int>(
                name: "AgentId",
                table: "Properties",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2025, 7, 22, 10, 53, 37, 557, DateTimeKind.Utc).AddTicks(4427));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2025, 7, 22, 10, 53, 37, 557, DateTimeKind.Utc).AddTicks(4433));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2025, 7, 22, 10, 53, 37, 557, DateTimeKind.Utc).AddTicks(4436));

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Agents_AgentId",
                table: "Properties",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "AgentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Agents_AgentId",
                table: "Properties");

            migrationBuilder.AlterColumn<int>(
                name: "AgentId",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2025, 7, 22, 10, 47, 9, 20, DateTimeKind.Utc).AddTicks(8035));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2025, 7, 22, 10, 47, 9, 20, DateTimeKind.Utc).AddTicks(8041));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2025, 7, 22, 10, 47, 9, 20, DateTimeKind.Utc).AddTicks(8043));

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Agents_AgentId",
                table: "Properties",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "AgentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
