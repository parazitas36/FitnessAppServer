using FitnessAppAPI.DTOs.TrainingPlan;

namespace FitnessAppAPI.Services.Interfaces;

public interface ITrainingPlanLogic
{
    Task<List<TrainingPlanShortGetDto>> GetTrainersTrainingPlanShortList(int trainerId);
    Task<bool> CreateTrainingPlan(int userId, TrainingPlanPostDto dto);
    Task<TrainingPlanGetDto?> GetTrainingPlanById(int userId, int trainingPlanId, bool trainer);
    Task<bool> AssignTrainingPlan(int trainerId, int clientId, int trainingPlanId);
    Task<List<TrainingPlanShortGetDto>> GetUsersTrainingPlanShortList(int userId);
}
