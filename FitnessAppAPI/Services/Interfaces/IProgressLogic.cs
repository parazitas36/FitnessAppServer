using FitnessAppAPI.DTOs.TrainingPlanProgress;

namespace FitnessAppAPI.Services.Interfaces
{
    public interface IProgressLogic
    {
        Task<List<TrainingPlanProgressGetDto>> GetTrainingPlanProgress(int trainerId, int trainingPlanId);
        Task<List<TrainingPlanProgressGetDto>> GetUserTrainingPlanProgress(int userId, int trainingPlanId);
    }
}