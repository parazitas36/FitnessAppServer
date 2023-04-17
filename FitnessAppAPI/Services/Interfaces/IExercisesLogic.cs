using FitnessAppAPI.DTOs.Exercise;

namespace FitnessAppAPI.Services.Interfaces
{
    public interface IExercisesLogic
    {
        Task<bool> CreateExercise(int userId, ExerciseWithGuidePostDto exerciseDto);
        Task<List<ExerciseGetDto>> GetTrainersExerices(int userId);
        Task<ExerciseWithGuideGetDto> GetExerciseById(int exerciseId);
    }
}