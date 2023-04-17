using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrainingPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ScheduledStartDate",
                table: "TrainingPlan_Exercise",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ScheduledEndDate",
                table: "TrainingPlan_Exercise",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "TrainingPlan_Exercise",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Week",
                table: "TrainingPlan_Exercise",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrainingPlanType",
                table: "TrainingPlan",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "TrainingPlan_Exercise");

            migrationBuilder.DropColumn(
                name: "Week",
                table: "TrainingPlan_Exercise");

            migrationBuilder.DropColumn(
                name: "TrainingPlanType",
                table: "TrainingPlan");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ScheduledStartDate",
                table: "TrainingPlan_Exercise",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ScheduledEndDate",
                table: "TrainingPlan_Exercise",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
