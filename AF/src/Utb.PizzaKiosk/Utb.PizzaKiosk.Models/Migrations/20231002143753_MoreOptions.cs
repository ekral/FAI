using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Utb.PizzaKiosk.Models.Migrations
{
    /// <inheritdoc />
    public partial class MoreOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DefaultValue",
                table: "PizzaOptions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaximalValue",
                table: "PizzaOptions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinimalValue",
                table: "PizzaOptions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantityOption_DefaultValue",
                table: "PizzaOptions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.InsertData(
                table: "PizzaOptions",
                columns: new[] { "PizzaConfigurationOptionId", "DefaultValue", "Description", "Discriminator" },
                values: new object[] { 2, true, "Garling", "BooleanOption" });

            migrationBuilder.InsertData(
                table: "PizzaOptions",
                columns: new[] { "PizzaConfigurationOptionId", "QuantityOption_DefaultValue", "Description", "Discriminator", "MaximalValue", "MinimalValue" },
                values: new object[] { 3, 1, "Number of pfeferoni", "QuantityOption", 10, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PizzaOptions",
                keyColumn: "PizzaConfigurationOptionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PizzaOptions",
                keyColumn: "PizzaConfigurationOptionId",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "PizzaOptions");

            migrationBuilder.DropColumn(
                name: "MaximalValue",
                table: "PizzaOptions");

            migrationBuilder.DropColumn(
                name: "MinimalValue",
                table: "PizzaOptions");

            migrationBuilder.DropColumn(
                name: "QuantityOption_DefaultValue",
                table: "PizzaOptions");
        }
    }
}
