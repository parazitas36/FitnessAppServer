using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrainingPlanDropScheduledDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduledEndDate",
                table: "TrainingPlan_Exercise");

            migrationBuilder.DropColumn(
                name: "ScheduledStartDate",
                table: "TrainingPlan_Exercise");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledEndDate",
                table: "TrainingPlan_Exercise",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledStartDate",
                table: "TrainingPlan_Exercise",
                type: "datetime2",
                nullable: true);
        }
    }
}
