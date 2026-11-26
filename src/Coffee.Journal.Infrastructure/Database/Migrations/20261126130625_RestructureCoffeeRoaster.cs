using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coffee.Journal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RestructureCoffeeRoaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_CoffeeRoasters_Rate",
                table: "CoffeeRoasters");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "CoffeeRoasters");

            migrationBuilder.DropColumn(
                name: "Recommend",
                table: "CoffeeRoasters");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "CoffeeRoasters");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "CoffeeRoasters",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "CoffeeRoasters",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "CoffeeRoasters",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CoffeeRoasters",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramUrl",
                table: "CoffeeRoasters",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "CoffeeRoasters",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "CoffeeRoasters",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CoffeeRoasterClassifications",
                columns: table => new
                {
                    RoasterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Classification = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeRoasterClassifications", x => new { x.RoasterId, x.Classification });
                    table.ForeignKey(
                        name: "FK_CoffeeRoasterClassifications_CoffeeRoasters_RoasterId",
                        column: x => x.RoasterId,
                        principalTable: "CoffeeRoasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeRoasterImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    RoasterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeRoasterImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoffeeRoasterImages_CoffeeRoasters_RoasterId",
                        column: x => x.RoasterId,
                        principalTable: "CoffeeRoasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeRoasterReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Review = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Recommend = table.Column<bool>(type: "bit", nullable: false),
                    RoasterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeRoasterReviews", x => x.Id);
                    table.CheckConstraint("CK_CoffeeRoasterReviews_Rating", "[Rating] BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_CoffeeRoasterReviews_CoffeeRoasters_RoasterId",
                        column: x => x.RoasterId,
                        principalTable: "CoffeeRoasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeRoasterImages_RoasterId",
                table: "CoffeeRoasterImages",
                column: "RoasterId");

            migrationBuilder.CreateIndex(
                name: "UX_CoffeeRoasterReviews_RoasterId_ProfileId",
                table: "CoffeeRoasterReviews",
                columns: new[] { "RoasterId", "ProfileId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeeRoasterClassifications");

            migrationBuilder.DropTable(
                name: "CoffeeRoasterImages");

            migrationBuilder.DropTable(
                name: "CoffeeRoasterReviews");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "CoffeeRoasters");

            migrationBuilder.DropColumn(
                name: "City",
                table: "CoffeeRoasters");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "CoffeeRoasters");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CoffeeRoasters");

            migrationBuilder.DropColumn(
                name: "InstagramUrl",
                table: "CoffeeRoasters");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "CoffeeRoasters");

            migrationBuilder.DropColumn(
                name: "State",
                table: "CoffeeRoasters");

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "CoffeeRoasters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Recommend",
                table: "CoffeeRoasters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "CoffeeRoasters",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddCheckConstraint(
                name: "CK_CoffeeRoasters_Rate",
                table: "CoffeeRoasters",
                sql: "[Rate] BETWEEN 1 AND 5");
        }
    }
}
