using FitnessAppAPI.DTOs.Exercise;

namespace FitnessAppAPI.Services.Interfaces
{
    public interface IExercisesLogic
    {
        Task<bool> CreateExercise(int userId, ExercisePostDto exerciseDto);
        Task<List<ExerciseGetDto>> GetTrainersExerices(int userId);
    }
}