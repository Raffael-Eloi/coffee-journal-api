using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coffee.Journal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCoffees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coffees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rate = table.Column<int>(type: "int", nullable: false),
                    Review = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Recommend = table.Column<bool>(type: "bit", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Process = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RoastLevel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Variety = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TastingNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coffees", x => x.Id);
                    table.CheckConstraint("CK_Coffees_Rate", "[Rate] BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_Coffees_CoffeeSuppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "CoffeeSuppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CoffeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoffeeImages_Coffees_CoffeeId",
                        column: x => x.CoffeeId,
                        principalTable: "Coffees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoffeePrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Grams = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    CoffeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeePrices", x => x.Id);
                    table.CheckConstraint("CK_CoffeePrices_Grams", "[Grams] > 0");
                    table.CheckConstraint("CK_CoffeePrices_Price", "[Price] > 0");
                    table.ForeignKey(
                        name: "FK_CoffeePrices_Coffees_CoffeeId",
                        column: x => x.CoffeeId,
                        principalTable: "Coffees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeImages_CoffeeId",
                table: "CoffeeImages",
                column: "CoffeeId");

            migrationBuilder.CreateIndex(
                name: "UX_CoffeePrices_CoffeeId_Grams",
                table: "CoffeePrices",
                columns: new[] { "CoffeeId", "Grams" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Coffees_SupplierId_Name",
                table: "Coffees",
                columns: new[] { "SupplierId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeeImages");

            migrationBuilder.DropTable(
                name: "CoffeePrices");

            migrationBuilder.DropTable(
                name: "Coffees");
        }
    }
}
