using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coffee.Journal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Roles",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Roles",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Profiles",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Profiles",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProfileRoles",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ProfileRoles",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoffeeShops",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CoffeeShops",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoffeeShopReviews",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CoffeeShopReviews",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoffeeShopOpeningHours",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CoffeeShopOpeningHours",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoffeeShopImages",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CoffeeShopImages",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Coffees",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Coffees",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoffeeRoasters",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CoffeeRoasters",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoffeeRoasterReviews",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CoffeeRoasterReviews",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoffeeRoasterImages",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CoffeeRoasterImages",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoffeeReviews",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CoffeeReviews",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoffeePrices",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CoffeePrices",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoffeeImages",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CoffeeImages",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoffeeEquipments",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CoffeeEquipments",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CoffeeEquipmentCategories",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CoffeeEquipmentCategories",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "CoffeeEquipmentCategories",
                keyColumn: "Id",
                keyValue: new Guid("a1f4c3a0-0001-4b1e-9c11-5d0a6e000001"),
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                table: "CoffeeEquipmentCategories",
                keyColumn: "Id",
                keyValue: new Guid("a1f4c3a0-0002-4b1e-9c11-5d0a6e000002"),
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                table: "CoffeeEquipmentCategories",
                keyColumn: "Id",
                keyValue: new Guid("a1f4c3a0-0003-4b1e-9c11-5d0a6e000003"),
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                table: "CoffeeEquipmentCategories",
                keyColumn: "Id",
                keyValue: new Guid("a1f4c3a0-0004-4b1e-9c11-5d0a6e000004"),
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                table: "CoffeeEquipmentCategories",
                keyColumn: "Id",
                keyValue: new Guid("a1f4c3a0-0005-4b1e-9c11-5d0a6e000005"),
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                table: "CoffeeEquipmentCategories",
                keyColumn: "Id",
                keyValue: new Guid("a1f4c3a0-0006-4b1e-9c11-5d0a6e000006"),
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                table: "CoffeeEquipmentCategories",
                keyColumn: "Id",
                keyValue: new Guid("a1f4c3a0-0007-4b1e-9c11-5d0a6e000007"),
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b2e5d4b1-0001-4c2f-8d22-6e1b7f000001"),
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b2e5d4b1-0002-4c2f-8d22-6e1b7f000002"),
                columns: new[] { "CreatedBy", "UpdatedBy" },
                values: new object[] { "system", "system" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProfileRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ProfileRoles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoffeeShops");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CoffeeShops");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoffeeShopReviews");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CoffeeShopReviews");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoffeeShopOpeningHours");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CoffeeShopOpeningHours");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoffeeShopImages");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CoffeeShopImages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Coffees");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Coffees");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoffeeRoasters");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CoffeeRoasters");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoffeeRoasterReviews");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CoffeeRoasterReviews");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoffeeRoasterImages");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CoffeeRoasterImages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoffeeReviews");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CoffeeReviews");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoffeePrices");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CoffeePrices");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoffeeImages");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CoffeeImages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoffeeEquipments");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CoffeeEquipments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CoffeeEquipmentCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CoffeeEquipmentCategories");
        }
    }
}
