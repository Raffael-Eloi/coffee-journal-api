﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coffee.Journal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RestructureCoffeeShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeShopImages_CoffeeShops_CoffeeShopId",
                table: "CoffeeShopImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeShops_CoffeeRoasters_CoffeeRoasterId",
                table: "CoffeeShops");

            migrationBuilder.DropIndex(
                name: "IX_CoffeeShops_CoffeeRoasterId",
                table: "CoffeeShops");

            migrationBuilder.DropCheckConstraint(
                name: "CK_CoffeeShops_Rate",
                table: "CoffeeShops");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "CoffeeShops");

            migrationBuilder.DropColumn(
                name: "Recommend",
                table: "CoffeeShops");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "CoffeeShops");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "CoffeeShops",
                newName: "Address");

            migrationBuilder.RenameIndex(
                name: "UX_CoffeeShops_Name_Location",
                table: "CoffeeShops",
                newName: "UX_CoffeeShops_Name_Address");

            migrationBuilder.RenameColumn(
                name: "CoffeeShopId",
                table: "CoffeeShopImages",
                newName: "ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_CoffeeShopImages_CoffeeShopId",
                table: "CoffeeShopImages",
                newName: "IX_CoffeeShopImages_ShopId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CoffeeShops",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramUrl",
                table: "CoffeeShops",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "CoffeeShops",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "CoffeeShops",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CoffeeShopAmenities",
                columns: table => new
                {
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amenity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeShopAmenities", x => new { x.ShopId, x.Amenity });
                    table.ForeignKey(
                        name: "FK_CoffeeShopAmenities_CoffeeShops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "CoffeeShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeShopBrewingMethods",
                columns: table => new
                {
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrewingMethod = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeShopBrewingMethods", x => new { x.ShopId, x.BrewingMethod });
                    table.ForeignKey(
                        name: "FK_CoffeeShopBrewingMethods_CoffeeShops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "CoffeeShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeShopOpeningHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    OpensAt = table.Column<TimeOnly>(type: "time", nullable: false),
                    ClosesAt = table.Column<TimeOnly>(type: "time", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeShopOpeningHours", x => x.Id);
                    table.CheckConstraint("CK_CoffeeShopOpeningHours_DayOfWeek", "[DayOfWeek] BETWEEN 0 AND 6");
                    table.CheckConstraint("CK_CoffeeShopOpeningHours_Hours", "[OpensAt] <> [ClosesAt]");
                    table.ForeignKey(
                        name: "FK_CoffeeShopOpeningHours_CoffeeShops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "CoffeeShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeShopReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Review = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Recommend = table.Column<bool>(type: "bit", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeShopReviews", x => x.Id);
                    table.CheckConstraint("CK_CoffeeShopReviews_Rating", "[Rating] BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_CoffeeShopReviews_CoffeeShops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "CoffeeShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeShopRoasters",
                columns: table => new
                {
                    RoasterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeShopRoasters", x => new { x.RoasterId, x.ShopId });
                    table.ForeignKey(
                        name: "FK_CoffeeShopRoasters_CoffeeRoasters_RoasterId",
                        column: x => x.RoasterId,
                        principalTable: "CoffeeRoasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoffeeShopRoasters_CoffeeShops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "CoffeeShops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Keep the existing shop -> roaster links before the old column is dropped.
            migrationBuilder.Sql(
                "INSERT INTO CoffeeShopRoasters (RoasterId, ShopId) SELECT CoffeeRoasterId, Id FROM CoffeeShops WHERE CoffeeRoasterId IS NOT NULL");

            migrationBuilder.DropColumn(
                name: "CoffeeRoasterId",
                table: "CoffeeShops");

            migrationBuilder.CreateIndex(
                name: "UX_CoffeeShopOpeningHours_ShopId_DayOfWeek_OpensAt",
                table: "CoffeeShopOpeningHours",
                columns: new[] { "ShopId", "DayOfWeek", "OpensAt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_CoffeeShopReviews_ShopId_ProfileId",
                table: "CoffeeShopReviews",
                columns: new[] { "ShopId", "ProfileId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeShopRoasters_ShopId",
                table: "CoffeeShopRoasters",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeShopImages_CoffeeShops_ShopId",
                table: "CoffeeShopImages",
                column: "ShopId",
                principalTable: "CoffeeShops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeShopImages_CoffeeShops_ShopId",
                table: "CoffeeShopImages");

            migrationBuilder.DropTable(
                name: "CoffeeShopAmenities");

            migrationBuilder.DropTable(
                name: "CoffeeShopBrewingMethods");

            migrationBuilder.DropTable(
                name: "CoffeeShopOpeningHours");

            migrationBuilder.DropTable(
                name: "CoffeeShopReviews");

            migrationBuilder.DropTable(
                name: "CoffeeShopRoasters");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CoffeeShops");

            migrationBuilder.DropColumn(
                name: "InstagramUrl",
                table: "CoffeeShops");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "CoffeeShops");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "CoffeeShops");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "CoffeeShops",
                newName: "Location");

            migrationBuilder.RenameIndex(
                name: "UX_CoffeeShops_Name_Address",
                table: "CoffeeShops",
                newName: "UX_CoffeeShops_Name_Location");

            migrationBuilder.RenameColumn(
                name: "ShopId",
                table: "CoffeeShopImages",
                newName: "CoffeeShopId");

            migrationBuilder.RenameIndex(
                name: "IX_CoffeeShopImages_ShopId",
                table: "CoffeeShopImages",
                newName: "IX_CoffeeShopImages_CoffeeShopId");

            migrationBuilder.AddColumn<Guid>(
                name: "CoffeeRoasterId",
                table: "CoffeeShops",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "CoffeeShops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Recommend",
                table: "CoffeeShops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "CoffeeShops",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeShops_CoffeeRoasterId",
                table: "CoffeeShops",
                column: "CoffeeRoasterId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CoffeeShops_Rate",
                table: "CoffeeShops",
                sql: "[Rate] BETWEEN 1 AND 5");

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeShopImages_CoffeeShops_CoffeeShopId",
                table: "CoffeeShopImages",
                column: "CoffeeShopId",
                principalTable: "CoffeeShops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeShops_CoffeeRoasters_CoffeeRoasterId",
                table: "CoffeeShops",
                column: "CoffeeRoasterId",
                principalTable: "CoffeeRoasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
