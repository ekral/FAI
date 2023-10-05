using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Utb.PizzaKiosk.Models.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PizzaOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    DefaultValueIndex = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PizzaOptions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PizzaOptions",
                columns: new[] { "Id", "DefaultValueIndex", "Description", "Discriminator" },
                values: new object[] { 1, 1, "Pizza size", "StringOptions" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PizzaOptions");
        }
    }
}
