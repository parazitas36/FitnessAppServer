using FitnessAppAPI.DTOs.Forms;

namespace FitnessAppAPI.Services.Interfaces
{
    public interface IFormsLogic
    {
        Task<List<BodyMeasurementsGetDto>> GetBodyMeasurements(int userId);
        Task<List<JobOfferGetDto>> GetJobOffers(int trainerId);
        Task<List<JobOfferGetDto>> GetSportsClubsJobOffers(int sportsClubAdminId);
        Task<List<TrainerJobFormGetDto>> GetTrainerJobForms(int trainerId);
        Task<List<TrainerJobFormGetDto>> GetTrainersJobForms();
        Task<List<TrainingPlanFormGetDto>> GetTrainingPlanForms();
        Task<List<TrainingPlanFormGetDto>> GetUsersTrainingPlanForms(int userId);
        Task<bool> PostJobOffer(int sportsClubAdminId, JobOfferPostDto dto);
        Task<bool> PostMeasurements(int userId, BodyMeasurementsPostDto dto);
        Task<bool> PostTrainerJobForm(int trainerId, TrainerJobFormPostDto dto);
        Task<bool> PostTrainingPlanForm(int userId, TrainingPlanFormPostDto dto);
    }
}