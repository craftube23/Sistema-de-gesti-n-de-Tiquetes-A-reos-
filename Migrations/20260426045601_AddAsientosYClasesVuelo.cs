using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Migrations
{
    /// <inheritdoc />
    public partial class AddAsientosYClasesVuelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AsientosEconomica",
                table: "Vuelos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AsientosEjecutiva",
                table: "Vuelos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AsientosPrimeraClase",
                table: "Vuelos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClasesVuelo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Multiplicador = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClasesVuelo", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Asientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Numero = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VueloId = table.Column<int>(type: "int", nullable: false),
                    ClaseVueloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asientos_ClasesVuelo_ClaseVueloId",
                        column: x => x.ClaseVueloId,
                        principalTable: "ClasesVuelo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Asientos_Vuelos_VueloId",
                        column: x => x.VueloId,
                        principalTable: "Vuelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "ClasesVuelo",
                columns: new[] { "Id", "Multiplicador", "Nombre" },
                values: new object[,]
                {
                    { 1, 1.0m, "Economica" },
                    { 2, 1.8m, "Ejecutiva" },
                    { 3, 3.0m, "Primera Clase" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asientos_ClaseVueloId",
                table: "Asientos",
                column: "ClaseVueloId");

            migrationBuilder.CreateIndex(
                name: "IX_Asientos_VueloId",
                table: "Asientos",
                column: "VueloId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asientos");

            migrationBuilder.DropTable(
                name: "ClasesVuelo");

            migrationBuilder.DropColumn(
                name: "AsientosEconomica",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "AsientosEjecutiva",
                table: "Vuelos");

            migrationBuilder.DropColumn(
                name: "AsientosPrimeraClase",
                table: "Vuelos");
        }
    }
}
