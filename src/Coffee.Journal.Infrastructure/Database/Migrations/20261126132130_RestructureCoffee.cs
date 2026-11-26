﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coffee.Journal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RestructureCoffee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Coffees_Rate",
                table: "Coffees");

            migrationBuilder.DropColumn(
                name: "Recommend",
                table: "Coffees");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "Coffees");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Coffees");

            // Existing coffees get 1 (Traditional) until they are classified.
            migrationBuilder.AddColumn<int>(
                name: "Classification",
                table: "Coffees",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "ProductUrl",
                table: "Coffees",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CoffeeReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Review = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Recommend = table.Column<bool>(type: "bit", nullable: false),
                    CoffeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeReviews", x => x.Id);
                    table.CheckConstraint("CK_CoffeeReviews_Rating", "[Rating] BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_CoffeeReviews_Coffees_CoffeeId",
                        column: x => x.CoffeeId,
                        principalTable: "Coffees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UX_CoffeeReviews_CoffeeId_ProfileId",
                table: "CoffeeReviews",
                columns: new[] { "CoffeeId", "ProfileId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeeReviews");

            migrationBuilder.DropColumn(
                name: "ProductUrl",
                table: "Coffees");

            migrationBuilder.DropColumn(
                name: "Classification",
                table: "Coffees");

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "Coffees",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<bool>(
                name: "Recommend",
                table: "Coffees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "Coffees",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Coffees_Rate",
                table: "Coffees",
                sql: "[Rate] BETWEEN 1 AND 5");
        }
    }
}
