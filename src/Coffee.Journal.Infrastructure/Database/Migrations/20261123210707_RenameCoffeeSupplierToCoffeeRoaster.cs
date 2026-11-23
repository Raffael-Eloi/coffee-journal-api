using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coffee.Journal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameCoffeeSupplierToCoffeeRoaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coffees_CoffeeSuppliers_SupplierId",
                table: "Coffees");

            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeShops_CoffeeSuppliers_CoffeeSupplierId",
                table: "CoffeeShops");

            migrationBuilder.RenameTable(
                name: "CoffeeSuppliers",
                newName: "CoffeeRoasters");

            migrationBuilder.Sql("EXEC sp_rename N'PK_CoffeeSuppliers', N'PK_CoffeeRoasters', N'OBJECT';");
            migrationBuilder.Sql("EXEC sp_rename N'CK_CoffeeSuppliers_Rate', N'CK_CoffeeRoasters_Rate', N'OBJECT';");

            migrationBuilder.RenameIndex(
                name: "UX_CoffeeSuppliers_Name",
                table: "CoffeeRoasters",
                newName: "UX_CoffeeRoasters_Name");

            migrationBuilder.RenameColumn(
                name: "CoffeeSupplierId",
                table: "CoffeeShops",
                newName: "CoffeeRoasterId");

            migrationBuilder.RenameIndex(
                name: "IX_CoffeeShops_CoffeeSupplierId",
                table: "CoffeeShops",
                newName: "IX_CoffeeShops_CoffeeRoasterId");

            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "Coffees",
                newName: "RoasterId");

            migrationBuilder.RenameIndex(
                name: "UX_Coffees_SupplierId_Name",
                table: "Coffees",
                newName: "UX_Coffees_RoasterId_Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Coffees_CoffeeRoasters_RoasterId",
                table: "Coffees",
                column: "RoasterId",
                principalTable: "CoffeeRoasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeShops_CoffeeRoasters_CoffeeRoasterId",
                table: "CoffeeShops",
                column: "CoffeeRoasterId",
                principalTable: "CoffeeRoasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coffees_CoffeeRoasters_RoasterId",
                table: "Coffees");

            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeShops_CoffeeRoasters_CoffeeRoasterId",
                table: "CoffeeShops");

            migrationBuilder.RenameIndex(
                name: "UX_Coffees_RoasterId_Name",
                table: "Coffees",
                newName: "UX_Coffees_SupplierId_Name");

            migrationBuilder.RenameColumn(
                name: "RoasterId",
                table: "Coffees",
                newName: "SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_CoffeeShops_CoffeeRoasterId",
                table: "CoffeeShops",
                newName: "IX_CoffeeShops_CoffeeSupplierId");

            migrationBuilder.RenameColumn(
                name: "CoffeeRoasterId",
                table: "CoffeeShops",
                newName: "CoffeeSupplierId");

            migrationBuilder.RenameIndex(
                name: "UX_CoffeeRoasters_Name",
                table: "CoffeeRoasters",
                newName: "UX_CoffeeSuppliers_Name");

            migrationBuilder.Sql("EXEC sp_rename N'CK_CoffeeRoasters_Rate', N'CK_CoffeeSuppliers_Rate', N'OBJECT';");
            migrationBuilder.Sql("EXEC sp_rename N'PK_CoffeeRoasters', N'PK_CoffeeSuppliers', N'OBJECT';");

            migrationBuilder.RenameTable(
                name: "CoffeeRoasters",
                newName: "CoffeeSuppliers");

            migrationBuilder.AddForeignKey(
                name: "FK_Coffees_CoffeeSuppliers_SupplierId",
                table: "Coffees",
                column: "SupplierId",
                principalTable: "CoffeeSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeShops_CoffeeSuppliers_CoffeeSupplierId",
                table: "CoffeeShops",
                column: "CoffeeSupplierId",
                principalTable: "CoffeeSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
