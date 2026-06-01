using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RyHCarRental.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleCrudConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Plate",
                table: "Vehicle",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicle_Plate",
                table: "Vehicle",
                column: "Plate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicle_Plate",
                table: "Vehicle");

            migrationBuilder.AlterColumn<string>(
                name: "Plate",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
