using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Coffee.Journal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCoffeeEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoffeeEquipmentCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeEquipmentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeEquipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Review = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    EquipmentCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "nchar(3)", fixedLength: true, maxLength: 3, nullable: true),
                    Link = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FrequencyOfUse = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    PurchaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeEquipments", x => x.Id);
                    table.CheckConstraint("CK_CoffeeEquipments_PriceCurrency", "([Price] IS NULL AND [Currency] IS NULL) OR ([Price] IS NOT NULL AND [Currency] IS NOT NULL)");
                    table.CheckConstraint("CK_CoffeeEquipments_Rating", "[Rating] BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_CoffeeEquipments_CoffeeEquipmentCategories_EquipmentCategoryId",
                        column: x => x.EquipmentCategoryId,
                        principalTable: "CoffeeEquipmentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CoffeeEquipmentCategories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a1f4c3a0-0001-4b1e-9c11-5d0a6e000001"), new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Burr and blade grinders, manual or electric.", "Grinder", new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1f4c3a0-0002-4b1e-9c11-5d0a6e000002"), new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Paper filters used to brew filter coffee.", "PaperFilter", new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1f4c3a0-0003-4b1e-9c11-5d0a6e000003"), new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Scales used to weigh coffee and water.", "Scale", new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1f4c3a0-0004-4b1e-9c11-5d0a6e000004"), new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Mugs and cups used to drink coffee.", "Mug", new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1f4c3a0-0005-4b1e-9c11-5d0a6e000005"), new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Kettles used to heat and pour water.", "Kettle", new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1f4c3a0-0006-4b1e-9c11-5d0a6e000006"), new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Brewing devices such as V60, AeroPress or espresso machines.", "Brewer", new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1f4c3a0-0007-4b1e-9c11-5d0a6e000007"), new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Any other coffee accessory.", "Accessory", new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "UX_CoffeeEquipmentCategories_Name",
                table: "CoffeeEquipmentCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeEquipments_EquipmentCategoryId",
                table: "CoffeeEquipments",
                column: "EquipmentCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeeEquipments");

            migrationBuilder.DropTable(
                name: "CoffeeEquipmentCategories");
        }
    }
}
