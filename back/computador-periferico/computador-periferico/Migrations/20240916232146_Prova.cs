using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace computador_periferico.Migrations
{
    /// <inheritdoc />
    public partial class Prova : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Computadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Cor = table.Column<string>(type: "TEXT", nullable: false),
                    DataFabricacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Computadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Perifericos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    ComputadorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perifericos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Perifericos_Computadores_ComputadorId",
                        column: x => x.ComputadorId,
                        principalTable: "Computadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Perifericos_ComputadorId",
                table: "Perifericos",
                column: "ComputadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Perifericos");

            migrationBuilder.DropTable(
                name: "Computadores");
        }
    }
}
