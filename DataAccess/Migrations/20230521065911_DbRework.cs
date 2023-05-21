using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DbRework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobOffer");

            migrationBuilder.DropTable(
                name: "TrainerJobForm");

            migrationBuilder.CreateTable(
                name: "TrainerInvite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvitedById = table.Column<int>(type: "int", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerInvite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerInvite_SportsClub_InvitedById",
                        column: x => x.InvitedById,
                        principalTable: "SportsClub",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainerInvite_User_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainerInvite_InvitedById",
                table: "TrainerInvite",
                column: "InvitedById");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerInvite_TrainerId",
                table: "TrainerInvite",
                column: "TrainerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainerInvite");

            migrationBuilder.CreateTable(
                name: "TrainerJobForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Education = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    OtherDetails = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PersonalAchievements = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerJobForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerJobForm_User_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SportsClubId = table.Column<int>(type: "int", nullable: false),
                    TrainerJobFormId = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    OfferDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobOffer_SportsClub_SportsClubId",
                        column: x => x.SportsClubId,
                        principalTable: "SportsClub",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobOffer_TrainerJobForm_TrainerJobFormId",
                        column: x => x.TrainerJobFormId,
                        principalTable: "TrainerJobForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOffer_SportsClubId",
                table: "JobOffer",
                column: "SportsClubId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOffer_TrainerJobFormId",
                table: "JobOffer",
                column: "TrainerJobFormId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerJobForm_TrainerId",
                table: "TrainerJobForm",
                column: "TrainerId");
        }
    }
}
