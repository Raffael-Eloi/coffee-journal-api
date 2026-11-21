using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coffee.Journal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCoffeeShops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoffeeShops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CoffeeSupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rate = table.Column<int>(type: "int", nullable: false),
                    Review = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Recommend = table.Column<bool>(type: "bit", nullable: false),
                    City = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    State = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeShops", x => x.Id);
                    table.CheckConstraint("CK_CoffeeShops_Rate", "[Rate] BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_CoffeeShops_CoffeeSuppliers_CoffeeSupplierId",
                        column: x => x.CoffeeSupplierId,
                        principalTable: "CoffeeSuppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeShopImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CoffeeShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeShopImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoffeeShopImages_CoffeeShops_CoffeeShopId",
                        column: x => x.CoffeeShopId,
                        principalTable: "CoffeeShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeShopImages_CoffeeShopId",
                table: "CoffeeShopImages",
                column: "CoffeeShopId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeShops_CoffeeSupplierId",
                table: "CoffeeShops",
                column: "CoffeeSupplierId");

            migrationBuilder.CreateIndex(
                name: "UX_CoffeeShops_Name_Location",
                table: "CoffeeShops",
                columns: new[] { "Name", "Location" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeeShopImages");

            migrationBuilder.DropTable(
                name: "CoffeeShops");
        }
    }
}
