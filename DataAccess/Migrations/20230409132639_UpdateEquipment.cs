using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SportsClubId",
                table: "Equipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_SportsClubId",
                table: "Equipment",
                column: "SportsClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_SportsClub_SportsClubId",
                table: "Equipment",
                column: "SportsClubId",
                principalTable: "SportsClub",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_SportsClub_SportsClubId",
                table: "Equipment");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_SportsClubId",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "SportsClubId",
                table: "Equipment");
        }
    }
}
