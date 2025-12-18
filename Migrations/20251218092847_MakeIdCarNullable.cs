using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class MakeIdCarNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formular_Fahrzeuge_IdCar",
                table: "Formular");
            
            migrationBuilder.AlterColumn<int>(
                name: "IdCar",
                table: "Formular",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
            
            migrationBuilder.AddForeignKey(
                name: "FK_Formular_Fahrzeuge_IdCar",
                table: "Formular",
                column: "IdCar",
                principalTable: "Fahrzeuge",
                principalColumn: "IdCar",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formular_Fahrzeuge_IdCar",
                table: "Formular");
            
            migrationBuilder.AlterColumn<int>(
                name: "IdCar",
                table: "Formular",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Formular_Fahrzeuge_IdCar",
                table: "Formular",
                column: "IdCar",
                principalTable: "Fahrzeuge",
                principalColumn: "IdCar",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
