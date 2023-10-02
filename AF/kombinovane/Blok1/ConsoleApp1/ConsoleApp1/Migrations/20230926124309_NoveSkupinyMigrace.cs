using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleApp1.Migrations
{
    /// <inheritdoc />
    public partial class NoveSkupinyMigrace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Skupina_SkupinaId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skupina",
                table: "Skupina");

            migrationBuilder.RenameTable(
                name: "Skupina",
                newName: "Skupiny");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skupiny",
                table: "Skupiny",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Skupiny_SkupinaId",
                table: "Students",
                column: "SkupinaId",
                principalTable: "Skupiny",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Skupiny_SkupinaId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skupiny",
                table: "Skupiny");

            migrationBuilder.RenameTable(
                name: "Skupiny",
                newName: "Skupina");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skupina",
                table: "Skupina",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Skupina_SkupinaId",
                table: "Students",
                column: "SkupinaId",
                principalTable: "Skupina",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
