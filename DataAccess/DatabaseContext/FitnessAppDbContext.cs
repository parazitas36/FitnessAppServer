namespace DataAccess.DatabaseContext;

using DataAccess.Models.ChatModels;
using DataAccess.Models.FormsModels;
using DataAccess.Models.SportsClubModels;
using DataAccess.Models.TrainingPlanModels;
using DataAccess.Models.UserModels;
using Microsoft.EntityFrameworkCore;

public class FitnessAppDbContext : DbContext
{
    public FitnessAppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<JobOffer> JobOffers { get; set; }
    public DbSet<TrainerJobForm> TrainerJobForms { get; set; }
    public DbSet<TrainingPlanForm> TrainingPlanForms { get; set; }
    public DbSet<TrainingPlanOffer> TrainingPlanOffers { get; set; }
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Facility> Facilities { get; set; }
    public DbSet<FacilityEquipment> FacilitiesEquipment { get; set; }
    public DbSet<SportsClub> SportsClubs { get; set; }
    public DbSet<SportsClubTrainer> SportsClubTrainers { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<TrainerFacility> TrainerFacilities { get; set; }
    public DbSet<ClientTrainingPlan> ClientTrainingPlans { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<ExerciseGuide> ExerciseGuides { get; set; }
    public DbSet<ExerciseProgress> ExerciseProgress { get; set; }
    public DbSet<TrainingPlanExercise> TrainingPlanExercises { get; set; }
    public DbSet<BodyMeasurements> BodyMeasurements { get; set; }
    public DbSet<ContactInfo> ContactInfo { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<PasswordReset> PasswordReset { get; set; }
}
