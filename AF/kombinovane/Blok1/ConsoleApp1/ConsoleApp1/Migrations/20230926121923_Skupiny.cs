using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConsoleApp1.Migrations
{
    /// <inheritdoc />
    public partial class Skupiny : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkupinaId",
                table: "Students",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Skupina",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazev = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skupina", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Skupina",
                columns: new[] { "Id", "Nazev" },
                values: new object[,]
                {
                    { 1, "swi1" },
                    { 2, "swi2" }
                });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1,
                column: "SkupinaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                column: "SkupinaId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3,
                column: "SkupinaId",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_Students_SkupinaId",
                table: "Students",
                column: "SkupinaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Skupina_SkupinaId",
                table: "Students",
                column: "SkupinaId",
                principalTable: "Skupina",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Skupina_SkupinaId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Skupina");

            migrationBuilder.DropIndex(
                name: "IX_Students_SkupinaId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SkupinaId",
                table: "Students");
        }
    }
}
