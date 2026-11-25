using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coffee.Journal.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCoffeeEquipmentRecommend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Recommend",
                table: "CoffeeEquipments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recommend",
                table: "CoffeeEquipments");
        }
    }
}
