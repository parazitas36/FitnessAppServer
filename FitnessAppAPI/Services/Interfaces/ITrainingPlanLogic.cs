namespace FitnessAppAPI.Services.Interfaces;


using FitnessAppAPI.DTOs.TrainingPlan;

public interface ITrainingPlanLogic
{
    Task<List<TrainingPlanShortGetDto>> GetTrainersTrainingPlanShortList(int trainerId);
    Task<bool> CreateTrainingPlan(int userId, TrainingPlanPostDto dto);
    Task<TrainingPlanGetDto?> GetTrainingPlanById(int userId, int trainingPlanId);
    Task<bool> AssignTrainingPlan(int trainerId, int clientId, int trainingPlanId);
    Task<List<TrainingPlanShortGetDto>> GetUsersTrainingPlanShortList(int userId);
    Task<UserTrainingPlanGetDto> GetUserTrainingPlanById(int userId, int trainingPlanId);
    Task<bool> LogExerciseSet(int userId, int trainingPlanExerciseId, string loggedSets);
    Task<List<TrainingPlanShortGetDto>> GetClientTrainingPlans(int trainerId, int clientId);
    Task<UserTrainingPlanGetDto?> GetClientTrainingPlanById(int trainerId, int userId, int trainingPlanId);
    Task<bool> CopyTrainingPlan(int trainerId, int trainingPlanId, string coppiedTrainingPlanName);
    Task<bool> UpdateTrainingPlanAddNewExercise(int trainerId, int trainingPlanId, TrainingPlanNewExerciseUpdateDto dto);
    Task<bool> UpdateTrainingPlanExercise(int trainerId, int trainingPlanExerciseId, string sets);
    Task<bool> DeleteTrainingPlanExercise(int trainerId, int trainingPlanExerciseId);
}
