using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rollen",
                columns: table => new
                {
                    IdRolle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rollen", x => x.IdRolle);
                });

            migrationBuilder.CreateTable(
                name: "Standorte",
                columns: table => new
                {
                    IdOrt = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ort = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standorte", x => x.IdOrt);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdRolle = table.Column<int>(type: "integer", nullable: false),
                    Vorname = table.Column<string>(type: "text", nullable: true),
                    Nachname = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_Users_Rollen_IdRolle",
                        column: x => x.IdRolle,
                        principalTable: "Rollen",
                        principalColumn: "IdRolle",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fahrzeuge",
                columns: table => new
                {
                    IdCar = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdOrt = table.Column<int>(type: "integer", nullable: false),
                    Marke = table.Column<string>(type: "text", nullable: true),
                    DatumVonKauf = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Farbe = table.Column<string>(type: "text", nullable: true),
                    Typ = table.Column<string>(type: "text", nullable: true),
                    Kennzeichnung = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fahrzeuge", x => x.IdCar);
                    table.ForeignKey(
                        name: "FK_Fahrzeuge_Standorte_IdOrt",
                        column: x => x.IdOrt,
                        principalTable: "Standorte",
                        principalColumn: "IdOrt",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Formular",
                columns: table => new
                {
                    IdForm = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    IdManager = table.Column<int>(type: "integer", nullable: false),
                    IdCar = table.Column<int>(type: "integer", nullable: false),
                    IdOrt = table.Column<int>(type: "integer", nullable: false),
                    Startdatum = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Enddatum = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartZeit = table.Column<TimeSpan>(type: "interval", nullable: true),
                    EndZeit = table.Column<TimeSpan>(type: "interval", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    GrundDerBuchung = table.Column<string>(type: "text", nullable: true),
                    NameVonManager = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formular", x => x.IdForm);
                    table.ForeignKey(
                        name: "FK_Formular_Fahrzeuge_IdCar",
                        column: x => x.IdCar,
                        principalTable: "Fahrzeuge",
                        principalColumn: "IdCar",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Formular_Standorte_IdOrt",
                        column: x => x.IdOrt,
                        principalTable: "Standorte",
                        principalColumn: "IdOrt",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Formular_Users_IdManager",
                        column: x => x.IdManager,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Formular_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fahrzeuge_IdOrt",
                table: "Fahrzeuge",
                column: "IdOrt");

            migrationBuilder.CreateIndex(
                name: "IX_Formular_IdCar",
                table: "Formular",
                column: "IdCar");

            migrationBuilder.CreateIndex(
                name: "IX_Formular_IdManager",
                table: "Formular",
                column: "IdManager");

            migrationBuilder.CreateIndex(
                name: "IX_Formular_IdOrt",
                table: "Formular",
                column: "IdOrt");

            migrationBuilder.CreateIndex(
                name: "IX_Formular_IdUser",
                table: "Formular",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdRolle",
                table: "Users",
                column: "IdRolle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Formular");

            migrationBuilder.DropTable(
                name: "Fahrzeuge");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Standorte");

            migrationBuilder.DropTable(
                name: "Rollen");
        }
    }
}
