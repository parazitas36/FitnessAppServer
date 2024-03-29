﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    ContactInfoId = table.Column<int>(type: "int", nullable: true),
                    UsesImperialSystem = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProfilePictureURI = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_ContactInfo_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "ContactInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BodyMeasurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Shoulders = table.Column<double>(type: "float", nullable: true),
                    Chest = table.Column<double>(type: "float", nullable: true),
                    Waist = table.Column<double>(type: "float", nullable: true),
                    Hips = table.Column<double>(type: "float", nullable: true),
                    MeasurementDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImperialSystem = table.Column<bool>(type: "bit", nullable: false),
                    ImageUri = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyMeasurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BodyMeasurements_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PasswordReset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VerificationCode = table.Column<int>(type: "int", nullable: false),
                    ValidTillDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordReset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordReset_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SportsClub",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUri = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportsClub", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SportsClub_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingPlan_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPlanForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    FormDetails = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPlanForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingPlanForm_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageURI = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SportsClubId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipment_SportsClub_SportsClubId",
                        column: x => x.SportsClubId,
                        principalTable: "SportsClub",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactInfoId = table.Column<int>(type: "int", nullable: false),
                    SportsClubId = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUri = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facility_ContactInfo_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "ContactInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facility_SportsClub_SportsClubId",
                        column: x => x.SportsClubId,
                        principalTable: "SportsClub",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    SportsClubId = table.Column<int>(type: "int", nullable: true),
                    TrainerId = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_SportsClub_SportsClubId",
                        column: x => x.SportsClubId,
                        principalTable: "SportsClub",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Review_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Review_User_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SportsClubTrainer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    SportsClubId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportsClubTrainer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SportsClubTrainer_SportsClub_SportsClubId",
                        column: x => x.SportsClubId,
                        principalTable: "SportsClub",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SportsClubTrainer_User_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SportsClubId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscription_SportsClub_SportsClubId",
                        column: x => x.SportsClubId,
                        principalTable: "SportsClub",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainerInvite_User_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Client_TrainingPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TrainingPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client_TrainingPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_TrainingPlan_TrainingPlan_TrainingPlanId",
                        column: x => x.TrainingPlanId,
                        principalTable: "TrainingPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Client_TrainingPlan_User_ClientId",
                        column: x => x.ClientId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPlanOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingPlanFormId = table.Column<int>(type: "int", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PriceFrom = table.Column<double>(type: "float", nullable: false),
                    PriceTo = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OfferedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPlanOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingPlanOffer_TrainingPlanForm_TrainingPlanFormId",
                        column: x => x.TrainingPlanFormId,
                        principalTable: "TrainingPlanForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainingPlanOffer_User_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MuscleGroups = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ExerciseType = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercise_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Exercise_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FacilityEquipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacilityId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityEquipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilityEquipment_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FacilityEquipment_Facility_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facility",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainerFacility",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    FacilityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerFacility", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerFacility_Facility_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facility",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainerFacility_SportsClubTrainer_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "SportsClubTrainer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseGuides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    Guide = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseGuides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseGuides_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPlan_Exercise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingPlanId = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: true),
                    Week = table.Column<int>(type: "int", nullable: true),
                    Sets = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPlan_Exercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingPlan_Exercise_Exercise_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainingPlan_Exercise_TrainingPlan_TrainingPlanId",
                        column: x => x.TrainingPlanId,
                        principalTable: "TrainingPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseProgress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingPlanExerciseId = table.Column<int>(type: "int", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoggedSets = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseProgress_TrainingPlan_Exercise_TrainingPlanExerciseId",
                        column: x => x.TrainingPlanExerciseId,
                        principalTable: "TrainingPlan_Exercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BodyMeasurements_UserId",
                table: "BodyMeasurements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_TrainingPlan_ClientId",
                table: "Client_TrainingPlan",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_TrainingPlan_TrainingPlanId",
                table: "Client_TrainingPlan",
                column: "TrainingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_SportsClubId",
                table: "Equipment",
                column: "SportsClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_CreatedById",
                table: "Exercise",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_EquipmentId",
                table: "Exercise",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseGuides_ExerciseId",
                table: "ExerciseGuides",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseProgress_TrainingPlanExerciseId",
                table: "ExerciseProgress",
                column: "TrainingPlanExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_ContactInfoId",
                table: "Facility",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_SportsClubId",
                table: "Facility",
                column: "SportsClubId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityEquipment_EquipmentId",
                table: "FacilityEquipment",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilityEquipment_FacilityId",
                table: "FacilityEquipment",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordReset_UserId",
                table: "PasswordReset",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_CreatedById",
                table: "Review",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Review_SportsClubId",
                table: "Review",
                column: "SportsClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_TrainerId",
                table: "Review",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_SportsClub_OwnerId",
                table: "SportsClub",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_SportsClubTrainer_SportsClubId",
                table: "SportsClubTrainer",
                column: "SportsClubId");

            migrationBuilder.CreateIndex(
                name: "IX_SportsClubTrainer_TrainerId",
                table: "SportsClubTrainer",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_SportsClubId",
                table: "Subscription",
                column: "SportsClubId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerFacility_FacilityId",
                table: "TrainerFacility",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerFacility_TrainerId",
                table: "TrainerFacility",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerInvite_InvitedById",
                table: "TrainerInvite",
                column: "InvitedById");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerInvite_TrainerId",
                table: "TrainerInvite",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlan_CreatedById",
                table: "TrainingPlan",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlan_Exercise_ExerciseId",
                table: "TrainingPlan_Exercise",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlan_Exercise_TrainingPlanId",
                table: "TrainingPlan_Exercise",
                column: "TrainingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlanForm_CreatedById",
                table: "TrainingPlanForm",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlanOffer_TrainerId",
                table: "TrainingPlanOffer",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlanOffer_TrainingPlanFormId",
                table: "TrainingPlanOffer",
                column: "TrainingPlanFormId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ContactInfoId",
                table: "User",
                column: "ContactInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BodyMeasurements");

            migrationBuilder.DropTable(
                name: "Client_TrainingPlan");

            migrationBuilder.DropTable(
                name: "ExerciseGuides");

            migrationBuilder.DropTable(
                name: "ExerciseProgress");

            migrationBuilder.DropTable(
                name: "FacilityEquipment");

            migrationBuilder.DropTable(
                name: "PasswordReset");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "TrainerFacility");

            migrationBuilder.DropTable(
                name: "TrainerInvite");

            migrationBuilder.DropTable(
                name: "TrainingPlanOffer");

            migrationBuilder.DropTable(
                name: "TrainingPlan_Exercise");

            migrationBuilder.DropTable(
                name: "Facility");

            migrationBuilder.DropTable(
                name: "SportsClubTrainer");

            migrationBuilder.DropTable(
                name: "TrainingPlanForm");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "TrainingPlan");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "SportsClub");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ContactInfo");
        }
    }
}
