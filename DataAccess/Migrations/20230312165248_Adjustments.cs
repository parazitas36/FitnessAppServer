using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Adjustments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "TrainerJobForm",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Facility",
                newName: "Coordinates");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "TrainerJobForm",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "TrainerJobForm",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Facility",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Facility",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "TrainerJobForm");

            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "TrainerJobForm");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Facility");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Facility");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "TrainerJobForm",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "Coordinates",
                table: "Facility",
                newName: "Location");
        }
    }
}
