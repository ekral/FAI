using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Utb.Studenti.Models.Migrations
{
    /// <inheritdoc />
    public partial class VychoziMigrace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skupiny",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazev = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skupiny", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Studenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SkupinaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Jmeno = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Studenti_Skupiny_SkupinaId",
                        column: x => x.SkupinaId,
                        principalTable: "Skupiny",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Skupiny",
                columns: new[] { "Id", "Nazev" },
                values: new object[] { 1, "swi1" });

            migrationBuilder.InsertData(
                table: "Studenti",
                columns: new[] { "Id", "Jmeno", "SkupinaId" },
                values: new object[] { 1, "Bohumil", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Studenti_SkupinaId",
                table: "Studenti",
                column: "SkupinaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Studenti");

            migrationBuilder.DropTable(
                name: "Skupiny");
        }
    }
}
