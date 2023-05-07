using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBodyMeasurements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Hip",
                table: "BodyMeasurements",
                newName: "Shoulders");

            migrationBuilder.RenameColumn(
                name: "Bust",
                table: "BodyMeasurements",
                newName: "Hips");

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "BodyMeasurements",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Chest",
                table: "BodyMeasurements",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Height",
                table: "BodyMeasurements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Chest",
                table: "BodyMeasurements");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "BodyMeasurements");

            migrationBuilder.RenameColumn(
                name: "Shoulders",
                table: "BodyMeasurements",
                newName: "Hip");

            migrationBuilder.RenameColumn(
                name: "Hips",
                table: "BodyMeasurements",
                newName: "Bust");

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "BodyMeasurements",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
