﻿// <auto-generated />
using System;
using DataAccess.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(FitnessAppDbContext))]
    partial class FitnessAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataAccess.Models.FormsModels.TrainerInvite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("InvitedById")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TrainerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("InvitedById");

                    b.HasIndex("TrainerId");

                    b.ToTable("TrainerInvite");
                });

            modelBuilder.Entity("DataAccess.Models.FormsModels.TrainingPlanForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("FormDetails")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("TrainingPlanForm");
                });

            modelBuilder.Entity("DataAccess.Models.FormsModels.TrainingPlanOffer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Details")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime>("OfferedDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("PriceFrom")
                        .HasColumnType("float");

                    b.Property<double>("PriceTo")
                        .HasColumnType("float");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TrainerId")
                        .HasColumnType("int");

                    b.Property<int>("TrainingPlanFormId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TrainerId");

                    b.HasIndex("TrainingPlanFormId");

                    b.ToTable("TrainingPlanOffer");
                });

            modelBuilder.Entity("DataAccess.Models.SportsClubModels.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ImageURI")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("SportsClubId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SportsClubId");

                    b.ToTable("Equipment");
                });

            modelBuilder.Entity("DataAccess.Models.SportsClubModels.Facility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ContactInfoId")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ImageUri")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("SportsClubId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContactInfoId");

                    b.HasIndex("SportsClubId");

                    b.ToTable("Facility");
                });

            modelBuilder.Entity("DataAccess.Models.SportsClubModels.FacilityEquipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("int");

                    b.Property<int>("FacilityId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("FacilityId");

                    b.ToTable("FacilityEquipment");
                });

            modelBuilder.Entity("DataAccess.Models.SportsClubModels.SportsClub", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoUri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("SportsClub");
                });

            modelBuilder.Entity("DataAccess.Models.SportsClubModels.SportsClubTrainer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("SportsClubId")
                        .HasColumnType("int");

                    b.Property<int>("TrainerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SportsClubId");

                    b.HasIndex("TrainerId");

                    b.ToTable("SportsClubTrainer");
                });

            modelBuilder.Entity("DataAccess.Models.SportsClubModels.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("SportsClubId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SportsClubId");

                    b.ToTable("Subscription");
                });

            modelBuilder.Entity("DataAccess.Models.SportsClubModels.TrainerFacility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FacilityId")
                        .HasColumnType("int");

                    b.Property<int>("TrainerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FacilityId");

                    b.HasIndex("TrainerId");

                    b.ToTable("TrainerFacility");
                });

            modelBuilder.Entity("DataAccess.Models.TrainingPlanModels.ClientTrainingPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("TrainingPlanId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("TrainingPlanId");

                    b.ToTable("Client_TrainingPlan");
                });

            modelBuilder.Entity("DataAccess.Models.TrainingPlanModels.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<int?>("EquipmentId")
                        .HasColumnType("int");

                    b.Property<int>("ExerciseType")
                        .HasColumnType("int");

                    b.Property<string>("MuscleGroups")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("EquipmentId");

                    b.ToTable("Exercise");
                });

            modelBuilder.Entity("DataAccess.Models.TrainingPlanModels.ExerciseGuide", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<string>("Guide")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.ToTable("ExerciseGuides");
                });

            modelBuilder.Entity("DataAccess.Models.TrainingPlanModels.ExerciseProgress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CompletionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LoggedSets")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TrainingPlanExerciseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TrainingPlanExerciseId");

                    b.ToTable("ExerciseProgress");
                });

            modelBuilder.Entity("DataAccess.Models.TrainingPlanModels.TrainingPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("TrainingPlan");
                });

            modelBuilder.Entity("DataAccess.Models.TrainingPlanModels.TrainingPlanExercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Day")
                        .HasColumnType("int");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<string>("Sets")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<int>("TrainingPlanId")
                        .HasColumnType("int");

                    b.Property<int?>("Week")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("TrainingPlanId");

                    b.ToTable("TrainingPlan_Exercise");
                });

            modelBuilder.Entity("DataAccess.Models.UserModels.BodyMeasurements", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Chest")
                        .HasColumnType("float");

                    b.Property<double>("Height")
                        .HasColumnType("float");

                    b.Property<double?>("Hips")
                        .HasColumnType("float");

                    b.Property<string>("ImageUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ImperialSystem")
                        .HasColumnType("bit");

                    b.Property<DateTime>("MeasurementDay")
                        .HasColumnType("datetime2");

                    b.Property<double?>("Shoulders")
                        .HasColumnType("float");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<double?>("Waist")
                        .HasColumnType("float");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("BodyMeasurements");
                });

            modelBuilder.Entity("DataAccess.Models.UserModels.ContactInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("ContactInfo");
                });

            modelBuilder.Entity("DataAccess.Models.UserModels.PasswordReset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ValidTillDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("VerificationCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PasswordReset");
                });

            modelBuilder.Entity("DataAccess.Models.UserModels.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("ReviewText")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("SportsClubId")
                        .HasColumnType("int");

                    b.Property<int?>("TrainerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("SportsClubId");

                    b.HasIndex("TrainerId");

                    b.ToTable("Review");
                });

            modelBuilder.Entity("DataAccess.Models.UserModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ContactInfoId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ProfilePictureURI")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("UsesImperialSystem")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ContactInfoId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("DataAccess.Models.FormsModels.TrainerInvite", b =>
                {
                    b.HasOne("DataAccess.Models.SportsClubModels.SportsClub", "InvitedBy")
                        .WithMany()
                        .HasForeignKey("InvitedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Models.UserModels.User", "Trainer")
                        .WithMany()
                        .HasForeignKey("TrainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InvitedBy");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("DataAccess.Models.FormsModels.TrainingPlanForm", b =>
                {
                    b.HasOne("DataAccess.Models.UserModels.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("DataAccess.Models.FormsModels.TrainingPlanOffer", b =>
                {
                    b.HasOne("DataAccess.Models.UserModels.User", "Trainer")
                        .WithMany()
                        .HasForeignKey("TrainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Models.FormsModels.TrainingPlanForm", "TrainingPlanForm")
                        .WithMany()
                        .HasForeignKey("TrainingPlanFormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trainer");

                    b.Navigation("TrainingPlanForm");
                });

            modelBuilder.Entity("DataAccess.Models.SportsClubModels.Equipment", b =>
                {
                    b.HasOne("DataAccess.Models.SportsClubModels.SportsClub", "SportsClub")
                        .WithMany()
                        .HasForeignKey("SportsClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SportsClub");
                });

            modelBuilder.Entity("DataAccess.Models.SportsClubModels.Facility", b =>
                {
                    b.HasOne("DataAccess.Models.UserModels.ContactInfo", "ContactInfo")
                        .WithMany()
                        .HasForeignKey("ContactInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Models.SportsClubModels.SportsClub", "SportsClub")
                        .WithMany()
                        .HasForeignKey("SportsClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContactInfo");

                    b.Navigation("SportsClub");
                });

            modelBuilder.Entity("DataAccess.Models.SportsClubModels.FacilityEquipment", b =>
                {
                    b.HasOne("DataAccess.Models.SportsClubModels.Equipment", "Equipment")
                        .WithMany()
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Models.SportsClubModels.Facility", "Facility")
                        .WithMany()
                        .HasForeignKey("FacilityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("Facility");
                });

            modelBuilder.Entity("DataAccess.Models.SportsClubModels.SportsClub", b =>
                {
                    b.HasOne("DataAccess.Models.UserModels.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("DataAccess.Models.SportsClubModels.SportsClubTrainer", b =>
                {
                    b.HasOne("DataAccess.Models.SportsClubModels.SportsClub", "SportsClub")
                        .WithMany()
                        .HasForeignKey("SportsClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Models.UserModels.User", "Trainer")
                        .WithMany()
                        .HasForeignKey("TrainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SportsClub");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("DataAccess.Models.SportsClubModels.Subscription", b =>
                {
                    b.HasOne("DataAccess.Models.SportsClubModels.SportsClub", "SportsClub")
                        .WithMany()
                        .HasForeignKey("SportsClubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SportsClub");
                });

            modelBuilder.Entity("DataAccess.Models.SportsClubModels.TrainerFacility", b =>
                {
                    b.HasOne("DataAccess.Models.SportsClubModels.Facility", "Facility")
                        .WithMany()
                        .HasForeignKey("FacilityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Models.SportsClubModels.SportsClubTrainer", "Trainer")
                        .WithMany()
                        .HasForeignKey("TrainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Facility");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("DataAccess.Models.TrainingPlanModels.ClientTrainingPlan", b =>
                {
                    b.HasOne("DataAccess.Models.UserModels.User", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Models.TrainingPlanModels.TrainingPlan", "TrainingPlan")
                        .WithMany()
                        .HasForeignKey("TrainingPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("TrainingPlan");
                });

            modelBuilder.Entity("DataAccess.Models.TrainingPlanModels.Exercise", b =>
                {
                    b.HasOne("DataAccess.Models.UserModels.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Models.SportsClubModels.Equipment", "Equipment")
                        .WithMany()
                        .HasForeignKey("EquipmentId");

                    b.Navigation("CreatedBy");

                    b.Navigation("Equipment");
                });

            modelBuilder.Entity("DataAccess.Models.TrainingPlanModels.ExerciseGuide", b =>
                {
                    b.HasOne("DataAccess.Models.TrainingPlanModels.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");
                });

            modelBuilder.Entity("DataAccess.Models.TrainingPlanModels.ExerciseProgress", b =>
                {
                    b.HasOne("DataAccess.Models.TrainingPlanModels.TrainingPlanExercise", "TrainingPlanExercise")
                        .WithMany()
                        .HasForeignKey("TrainingPlanExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrainingPlanExercise");
                });

            modelBuilder.Entity("DataAccess.Models.TrainingPlanModels.TrainingPlan", b =>
                {
                    b.HasOne("DataAccess.Models.UserModels.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("DataAccess.Models.TrainingPlanModels.TrainingPlanExercise", b =>
                {
                    b.HasOne("DataAccess.Models.TrainingPlanModels.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Models.TrainingPlanModels.TrainingPlan", "TrainingPlan")
                        .WithMany()
                        .HasForeignKey("TrainingPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("TrainingPlan");
                });

            modelBuilder.Entity("DataAccess.Models.UserModels.BodyMeasurements", b =>
                {
                    b.HasOne("DataAccess.Models.UserModels.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Models.UserModels.PasswordReset", b =>
                {
                    b.HasOne("DataAccess.Models.UserModels.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Models.UserModels.Review", b =>
                {
                    b.HasOne("DataAccess.Models.UserModels.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Models.SportsClubModels.SportsClub", "SportsClub")
                        .WithMany()
                        .HasForeignKey("SportsClubId");

                    b.HasOne("DataAccess.Models.UserModels.User", "Trainer")
                        .WithMany()
                        .HasForeignKey("TrainerId");

                    b.Navigation("CreatedBy");

                    b.Navigation("SportsClub");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("DataAccess.Models.UserModels.User", b =>
                {
                    b.HasOne("DataAccess.Models.UserModels.ContactInfo", "ContactInfo")
                        .WithMany()
                        .HasForeignKey("ContactInfoId");

                    b.Navigation("ContactInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
