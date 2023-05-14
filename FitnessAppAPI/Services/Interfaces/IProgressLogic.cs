using FitnessAppAPI.DTOs.TrainingPlanProgress;

namespace FitnessAppAPI.Services.Interfaces
{
    public interface IProgressLogic
    {
        Task<List<TrainingPlanProgressGetDto>> GetTrainingPlanProgress(int trainerId, int trainingPlanId);
    }
}