using DataAccess.Enumerators;
using FitnessAppAPI.DTOs.Forms;

namespace FitnessAppAPI.Services.Interfaces
{
    public interface IFormsLogic
    {
        Task<List<BodyMeasurementsGetDto>> GetBodyMeasurements(int userId);
        Task<List<TrainingPlanFormGetDto>> GetTrainingPlanForms(int trainerId);
        Task<List<TrainingPlanFormGetDto>> GetUsersTrainingPlanForms(int userId);
        Task<bool> PostMeasurements(int userId, BodyMeasurementsPostDto dto);
        Task<bool> PostTrainingPlanForm(int userId, TrainingPlanFormPostDto dto);
        Task<List<TrainingPlanOfferGetDto>> GetTrainingPlanOffers(int userId, bool trainer = false);
        Task<bool> PostTrainingPlanOffer(int trainerId, TrainingPlanOfferPostDto dto);
        Task<bool> UpdateTrainingPlanOffer(int userId, int trainingPlanId, OfferStatus newStatus);
        Task<bool> DeleteTrainingPlanOffer(int trainerId, int trainingPlanId);
        Task<bool> PostTrainerInvite(int sportsClubAdminId, int trainerId);
        Task<List<TrainerInviteGetDto>> GetTrainerInvites(int trainerId);
        Task<bool> ChangeTrainerInviteStatus(int trainerId, int trainerInviteId, OfferStatus status);
        Task<List<BodyMeasurementsGetDto>> GetClientBodyMeasurements(int trainerId, int clientId);
    }
}