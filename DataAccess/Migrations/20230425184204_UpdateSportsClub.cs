using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSportsClub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "TrainerJobForm");

            migrationBuilder.RenameColumn(
                name: "MeasureMentDay",
                table: "BodyMeasurements",
                newName: "MeasurementDay");

            migrationBuilder.AddColumn<string>(
                name: "LogoUri",
                table: "SportsClub",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUri",
                table: "SportsClub");

            migrationBuilder.RenameColumn(
                name: "MeasurementDay",
                table: "BodyMeasurements",
                newName: "MeasureMentDay");

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "TrainerJobForm",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
