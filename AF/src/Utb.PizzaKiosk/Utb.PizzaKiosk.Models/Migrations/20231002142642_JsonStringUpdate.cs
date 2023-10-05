using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Utb.PizzaKiosk.Models.Migrations
{
    /// <inheritdoc />
    public partial class JsonStringUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "PizzaOptions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PizzaOptions",
                keyColumn: "PizzaConfigurationOptionId",
                keyValue: 1,
                column: "Options",
                value: "[\"Small\",\"Medium\",\"Large\"]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Options",
                table: "PizzaOptions");
        }
    }
}
