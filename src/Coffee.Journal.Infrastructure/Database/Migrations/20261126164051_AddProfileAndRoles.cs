using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Coffee.Journal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                table: "CoffeeEquipments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    Handle = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    InstagramUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FacebookUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    XUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LanguagePref = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
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
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfileRoles",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileRoles", x => new { x.RoleId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_ProfileRoles_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("b2e5d4b1-0001-4c2f-8d22-6e1b7f000001"), new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), "Everything, including delete and managing roles.", "Admin", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b2e5d4b1-0002-4c2f-8d22-6e1b7f000002"), new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), "Can create and update roasters, shops and coffees.", "Contributor", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            // Equipment saved before profiles existed has no owner. Assign it to a placeholder profile
            // (only created when there is equipment) so the new foreign key can be added.
            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM CoffeeEquipments)
BEGIN
    DECLARE @placeholder UNIQUEIDENTIFIER = '00000000-0000-0000-0000-00000000f001';

    INSERT INTO Profiles (Id, ExternalId, Name, Email, Handle, LanguagePref, CreatedAt, UpdatedAt)
    VALUES (@placeholder, 'unassigned', 'Unassigned equipment', 'unassigned@invalid.local', 'unassigned', 'pt-BR', SYSUTCDATETIME(), SYSUTCDATETIME());

    UPDATE CoffeeEquipments SET ProfileId = @placeholder;
END");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeShopReviews_ProfileId",
                table: "CoffeeShopReviews",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeRoasterReviews_ProfileId",
                table: "CoffeeRoasterReviews",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeReviews_ProfileId",
                table: "CoffeeReviews",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeEquipments_ProfileId",
                table: "CoffeeEquipments",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileRoles_ProfileId",
                table: "ProfileRoles",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "UX_Profiles_Email",
                table: "Profiles",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Profiles_ExternalId",
                table: "Profiles",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Profiles_Handle",
                table: "Profiles",
                column: "Handle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeEquipments_Profiles_ProfileId",
                table: "CoffeeEquipments",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeReviews_Profiles_ProfileId",
                table: "CoffeeReviews",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeRoasterReviews_Profiles_ProfileId",
                table: "CoffeeRoasterReviews",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeShopReviews_Profiles_ProfileId",
                table: "CoffeeShopReviews",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeEquipments_Profiles_ProfileId",
                table: "CoffeeEquipments");

            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeReviews_Profiles_ProfileId",
                table: "CoffeeReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeRoasterReviews_Profiles_ProfileId",
                table: "CoffeeRoasterReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeShopReviews_Profiles_ProfileId",
                table: "CoffeeShopReviews");

            migrationBuilder.DropTable(
                name: "ProfileRoles");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_CoffeeShopReviews_ProfileId",
                table: "CoffeeShopReviews");

            migrationBuilder.DropIndex(
                name: "IX_CoffeeRoasterReviews_ProfileId",
                table: "CoffeeRoasterReviews");

            migrationBuilder.DropIndex(
                name: "IX_CoffeeReviews_ProfileId",
                table: "CoffeeReviews");

            migrationBuilder.DropIndex(
                name: "IX_CoffeeEquipments_ProfileId",
                table: "CoffeeEquipments");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "CoffeeEquipments");
        }
    }
}
