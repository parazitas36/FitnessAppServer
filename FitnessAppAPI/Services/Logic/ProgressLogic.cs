using DataAccess.DatabaseContext;
using FitnessAppAPI.DTOs.TrainingPlanProgress;
using FitnessAppAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessAppAPI.Services.Logic;

public class ProgressLogic : IProgressLogic
{
    private readonly FitnessAppDbContext _dbContext;

    public ProgressLogic(FitnessAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TrainingPlanProgressGetDto>> GetTrainingPlanProgress(int trainerId, int trainingPlanId)
    {
        var result = await _dbContext.TrainingPlanExercises.Where(x => x.TrainingPlan.CreatedBy.Id == trainerId && x.TrainingPlan.Id == trainingPlanId)
            .GroupBy(x => x.Exercise)
            .Select(x => x.Key)
            .Select(x => new TrainingPlanProgressGetDto
            {
                ExerciseId = x.Id,
                ExerciseName = x.Name,
                Weeks = _dbContext.TrainingPlanExercises.Where(y => y.TrainingPlan.Id == trainingPlanId && y.Exercise.Id == x.Id)
                .GroupBy(y => y.Week)
                .Select(y => y.Key)
                .OrderBy(y => y)
                .Select(y => new WeekProgressGetDto
                {
                    Week = y.Value,
                    Days = _dbContext.TrainingPlanExercises.Where(z => z.TrainingPlan.Id == trainingPlanId && z.Exercise.Id == x.Id && z.Week == y.Value)
                    .GroupBy(z => z.Day)
                    .Select(z => z.Key)
                    .OrderBy(z => z)
                    .Select(z => new DayProgressGetDto
                    {
                        Day = z.Value.ToString(),
                        Sets = _dbContext.TrainingPlanExercises
                            .FirstOrDefault(zz => zz.TrainingPlan.Id == trainingPlanId && zz.Exercise.Id == x.Id && zz.Week == y.Value && zz.Day == z.Value).Sets,
                        LoggedSets = _dbContext.ExerciseProgress
                            .FirstOrDefault(zz => zz.TrainingPlanExercise.TrainingPlan.Id == trainingPlanId && zz.TrainingPlanExercise.Exercise.Id == x.Id 
                                            && zz.TrainingPlanExercise.Week == y.Value && zz.TrainingPlanExercise.Day == z.Value).LoggedSets
                    }).ToList()
                }).ToList()
            }).ToListAsync();

        return Task.FromResult(result).Result;
    }

    public async Task<List<TrainingPlanProgressGetDto>> GetUserTrainingPlanProgress(int userId, int trainingPlanId)
    {
        var assignedPlan = await _dbContext.ClientTrainingPlans.FirstOrDefaultAsync(x => x.Client.Id == userId && x.TrainingPlan.Id == trainingPlanId);

        if (assignedPlan == null) { return null; }

        var result = await _dbContext.TrainingPlanExercises.Where(x => x.TrainingPlan.Id == trainingPlanId)
            .GroupBy(x => x.Exercise)
            .Select(x => x.Key)
            .Select(x => new TrainingPlanProgressGetDto
            {
                ExerciseId = x.Id,
                ExerciseName = x.Name,
                Weeks = _dbContext.TrainingPlanExercises.Where(y => y.TrainingPlan.Id == trainingPlanId && y.Exercise.Id == x.Id)
                .GroupBy(y => y.Week)
                .Select(y => y.Key)
                .OrderBy(y => y)
                .Select(y => new WeekProgressGetDto
                {
                    Week = y.Value,
                    Days = _dbContext.TrainingPlanExercises.Where(z => z.TrainingPlan.Id == trainingPlanId && z.Exercise.Id == x.Id && z.Week == y.Value)
                    .GroupBy(z => z.Day)
                    .Select(z => z.Key)
                    .OrderBy(z => z)
                    .Select(z => new DayProgressGetDto
                    {
                        Day = z.Value.ToString(),
                        Sets = _dbContext.TrainingPlanExercises
                            .FirstOrDefault(zz => zz.TrainingPlan.Id == trainingPlanId && zz.Exercise.Id == x.Id && zz.Week == y.Value && zz.Day == z.Value).Sets,
                        LoggedSets = _dbContext.ExerciseProgress
                            .FirstOrDefault(zz => zz.TrainingPlanExercise.TrainingPlan.Id == trainingPlanId && zz.TrainingPlanExercise.Exercise.Id == x.Id
                                            && zz.TrainingPlanExercise.Week == y.Value && zz.TrainingPlanExercise.Day == z.Value).LoggedSets
                    }).ToList()
                }).ToList()
            }).ToListAsync();

        return Task.FromResult(result).Result;
    }
}
