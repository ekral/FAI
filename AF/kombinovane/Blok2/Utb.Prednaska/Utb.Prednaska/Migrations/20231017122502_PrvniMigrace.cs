using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Utb.Prednaska.Migrations
{
    /// <inheritdoc />
    public partial class PrvniMigrace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Incredience",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incredience", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PizzaStyle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PizzaStyle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pizzas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Cena = table.Column<double>(type: "REAL", nullable: false),
                    PizzaStyleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pizzas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pizzas_PizzaStyle_PizzaStyleId",
                        column: x => x.PizzaStyleId,
                        principalTable: "PizzaStyle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PizzaIncredience",
                columns: table => new
                {
                    PizzaId = table.Column<int>(type: "INTEGER", nullable: false),
                    IncredienceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PizzaIncredience", x => new { x.IncredienceId, x.PizzaId });
                    table.ForeignKey(
                        name: "FK_PizzaIncredience_Incredience_IncredienceId",
                        column: x => x.IncredienceId,
                        principalTable: "Incredience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PizzaIncredience_Pizzas_PizzaId",
                        column: x => x.PizzaId,
                        principalTable: "Pizzas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Incredience",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Cibule" },
                    { 2, "Hranolky" }
                });

            migrationBuilder.InsertData(
                table: "PizzaStyle",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "Italsky styl" },
                    { 2, "Americky styl" }
                });

            migrationBuilder.InsertData(
                table: "Pizzas",
                columns: new[] { "Id", "Cena", "Name", "PizzaStyleId" },
                values: new object[,]
                {
                    { 1, 100.0, "Margherita", 1 },
                    { 2, 130.0, "Salami", 1 },
                    { 3, 135.0, "Funghi", 2 }
                });

            migrationBuilder.InsertData(
                table: "PizzaIncredience",
                columns: new[] { "IncredienceId", "PizzaId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PizzaIncredience_PizzaId",
                table: "PizzaIncredience",
                column: "PizzaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pizzas_PizzaStyleId",
                table: "Pizzas",
                column: "PizzaStyleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PizzaIncredience");

            migrationBuilder.DropTable(
                name: "Incredience");

            migrationBuilder.DropTable(
                name: "Pizzas");

            migrationBuilder.DropTable(
                name: "PizzaStyle");
        }
    }
}
