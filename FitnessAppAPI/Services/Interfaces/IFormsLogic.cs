using DataAccess.Enumerators;
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
        Task<List<TrainingPlanFormGetDto>> GetTrainingPlanForms(int trainerId);
        Task<List<TrainingPlanFormGetDto>> GetUsersTrainingPlanForms(int userId);
        Task<bool> PostJobOffer(int sportsClubAdminId, JobOfferPostDto dto);
        Task<bool> PostMeasurements(int userId, BodyMeasurementsPostDto dto);
        Task<bool> PostTrainerJobForm(int trainerId, TrainerJobFormPostDto dto);
        Task<bool> PostTrainingPlanForm(int userId, TrainingPlanFormPostDto dto);
        Task<List<TrainingPlanOfferGetDto>> GetTrainingPlanOffers(int userId, bool trainer = false);
        Task<bool> PostTrainingPlanOffer(int trainerId, TrainingPlanOfferPostDto dto);
        Task<bool> UpdateTrainingPlanOffer(int userId, int trainingPlanId, OfferStatus newStatus);
        Task<bool> DeleteTrainingPlanOffer(int trainerId, int trainingPlanId);
    }
}