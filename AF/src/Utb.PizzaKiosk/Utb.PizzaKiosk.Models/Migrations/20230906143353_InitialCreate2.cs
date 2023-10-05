using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Utb.PizzaKiosk.Models.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PizzaOptions",
                newName: "PizzaConfigurationOptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PizzaConfigurationOptionId",
                table: "PizzaOptions",
                newName: "Id");
        }
    }
}
