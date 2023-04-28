using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFacility1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "Facility");

            migrationBuilder.RenameColumn(
                name: "PictureURI",
                table: "BodyMeasurements",
                newName: "ImageUri");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUri",
                table: "BodyMeasurements",
                newName: "PictureURI");

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "Facility",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
