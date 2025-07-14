using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sheltos.Data.Migrations
{
    /// <inheritdoc />
    public partial class newmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    AgentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.AgentId);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Beds = table.Column<int>(type: "int", nullable: false),
                    Bathrooms = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PropertyStatus = table.Column<int>(type: "int", nullable: true),
                    AgentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Properties_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "AgentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyFeatures",
                columns: table => new
                {
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyFeatures", x => new { x.PropertyId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_PropertyFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyFeatures_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyImages_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "AddressId", "City", "Country", "State", "ZipCode" },
                values: new object[,]
                {
                    { 1, "ojo", "Nigeria", "Lagos", 0 },
                    { 2, "New heaven", "Brazil", "Enugu", 0 },
                    { 3, "Tempsite", "Nigeria", "Anambra", 0 }
                });

            migrationBuilder.InsertData(
                table: "Agents",
                columns: new[] { "AgentId", "Address", "Email", "FullName", "ImageUrl", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "iba,ojo,lagos", "john@example.com", "John Doe", "/assets/images/avatar/3.jpg", "08012345678" },
                    { 2, "New heaven,enugu", "nwekeblessing06@gmail.com", "Gennie Doe", "/assets/images/avatar/3.jpg", "09057247888" }
                });

            migrationBuilder.InsertData(
                table: "Features",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Free Wi-Fi" },
                    { 2, "Power Supply" },
                    { 3, "Constant Water Supply" },
                    { 4, "Security Guard" },
                    { 5, "Elevator lift" },
                    { 6, "CCTV" },
                    { 7, "Laundry Service" }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "AddressId", "AgentId", "Bathrooms", "Beds", "DateTime", "Description", "Price", "PropertyStatus", "Title", "Type" },
                values: new object[,]
                {
                    { 1, 3, 1, 3, 4, new DateTime(2025, 7, 9, 18, 25, 55, 698, DateTimeKind.Utc).AddTicks(9182), "Residences can be classified by and how they are connected residences and land. Different types of housing tenure can be used for the same physical type.", 125000.0, 1, "Luxury Beachfront Villa", "Duplex" },
                    { 2, 2, 1, 3, 3, new DateTime(2025, 7, 9, 18, 25, 55, 698, DateTimeKind.Utc).AddTicks(9186), "An interior designer is someone who plans,researches,coordinates,management and manages such enhancement projects.", 300000000.0, 0, "Hidden Spring Hideway", "Apartment" },
                    { 3, 1, 2, 1, 2, new DateTime(2025, 7, 9, 18, 25, 55, 698, DateTimeKind.Utc).AddTicks(9188), "An apartment is a self-contained housing unit that occupies only part of a building, typically on a single level.", 75000.0, 1, "Modern City Apartment", "Apartment" }
                });

            migrationBuilder.InsertData(
                table: "PropertyFeatures",
                columns: new[] { "FeatureId", "PropertyId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 5, 1 },
                    { 2, 2 },
                    { 3, 2 },
                    { 4, 2 },
                    { 6, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 4, 3 }
                });

            migrationBuilder.InsertData(
                table: "PropertyImages",
                columns: new[] { "Id", "ImageUrl", "PropertyId" },
                values: new object[,]
                {
                    { 1, "/assets/images/property/10.jpg", 1 },
                    { 2, "/assets/images/property/5.jpg", 1 },
                    { 3, "/assets/images/property/3.jpg", 1 },
                    { 4, "/assets/images/property/4.jpg", 1 },
                    { 5, "/assets/images/property/10.jpg", 2 },
                    { 6, "/assets/images/property/5.jpg", 2 },
                    { 7, "/assets/images/property/3.jpg", 2 },
                    { 8, "/assets/images/property/4.jpg", 2 },
                    { 9, "/assets/images/property/10.jpg", 3 },
                    { 10, "/assets/images/property/5.jpg", 3 },
                    { 11, "/assets/images/property/3.jpg", 3 },
                    { 12, "/assets/images/property/4.jpg", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_AddressId",
                table: "Properties",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_AgentId",
                table: "Properties",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyFeatures_FeatureId",
                table: "PropertyFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImages_PropertyId",
                table: "PropertyImages",
                column: "PropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyFeatures");

            migrationBuilder.DropTable(
                name: "PropertyImages");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Agents");
        }
    }
}
