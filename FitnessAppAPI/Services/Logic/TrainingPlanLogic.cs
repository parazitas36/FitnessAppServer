namespace FitnessAppAPI.Services.Logic;

using DataAccess.DatabaseContext;
using DataAccess.Enumerators;
using DataAccess.Models.TrainingPlanModels;
using FitnessAppAPI.DTOs.TrainingPlan;
using FitnessAppAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

public class TrainingPlanLogic : ITrainingPlanLogic
{
    private readonly FitnessAppDbContext _dbContext;

    public TrainingPlanLogic(FitnessAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateTrainingPlan(int userId, TrainingPlanPostDto dto)
    {
        try
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) { return Task.FromResult(false).Result; }

            TrainingPlanType trainingPlanType;

            if (!Enum.TryParse<TrainingPlanType>(dto.Type, out trainingPlanType)) { return Task.FromResult(false).Result; }

            var createdTrainingPlan = (await _dbContext.TrainingPlans.AddAsync(new TrainingPlan
            {
                CreatedBy = user,
                Name = dto.Name,
                TrainingPlanType = trainingPlanType,
            })).Entity;

            switch (trainingPlanType)
            {
                case TrainingPlanType.Weekly:
                    return Task.FromResult(await this.ProcessWeeklyTrainingPlan(dto, createdTrainingPlan)).Result;
                case TrainingPlanType.Scheduled:
                    return Task.FromResult(await this.ProcessScheduledTrainingPlan(dto, createdTrainingPlan)).Result;
            }

            return Task.FromResult(false).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }

    public async Task<List<TrainingPlanShortGetDto>> GetTrainersTrainingPlanShortList(int trainerId)
    {
        try
        {
            var query = await (from trainingPlans in _dbContext.TrainingPlans.Where(x => x.CreatedBy.Id == trainerId)
                               from tpExercises in _dbContext.TrainingPlanExercises.Where(x => x.TrainingPlan.Id == trainingPlans.Id)
                               from exercise in _dbContext.Exercises.Where(x => x.Id == tpExercises.Exercise.Id)
                               from equipment in _dbContext.Equipment.Where(x => x.Id == exercise.Id).DefaultIfEmpty()
                               select new TrainingPlanShortGetDto
                               {
                                   CreatedById = trainerId,
                                   Name = trainingPlans.Name,
                                   MuscleGroups = exercise.MuscleGroups,
                                   ClientsCount = _dbContext.ClientTrainingPlans.Count(x => x.TrainingPlan.Id == trainingPlans.Id),
                                   Type = trainingPlans.TrainingPlanType.ToString(),
                               }).ToListAsync();

            return Task.FromResult(query).Result;
        }
        catch
        {
            return null;
        }
    }

    private async Task<bool> ProcessScheduledTrainingPlan(TrainingPlanPostDto dto, TrainingPlan createdTrainingPlan)
    {
        try
        {
            var trainingPlanExercisesIds = new List<int>();

            trainingPlanExercisesIds = dto.ScheduledPlan.Select(x => x.Id).Distinct().ToList();

            var exercises = await _dbContext.Exercises.Where(x => trainingPlanExercisesIds.Contains(x.Id)).ToListAsync();

            if (exercises.Count != trainingPlanExercisesIds.Count)
            {
                return Task.FromResult(false).Result;
            }

            var trainingPlanExercises = new List<TrainingPlanExercise>();

            foreach (var exercise in dto.ScheduledPlan)
            {
                trainingPlanExercises.Add(new TrainingPlanExercise
                {
                    Exercise = exercises.First(x => x.Id == exercise.Id),
                    ScheduledStartDate = Convert.ToDateTime(exercise.StartDate),
                    ScheduledEndDate = Convert.ToDateTime(exercise.EndDate),
                    Sets = exercise.Sets,
                    TrainingPlan = createdTrainingPlan,
                });
            }

            await _dbContext.AddRangeAsync(trainingPlanExercises);
            await _dbContext.SaveChangesAsync();

            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }

    private async Task<bool> ProcessWeeklyTrainingPlan(TrainingPlanPostDto dto, TrainingPlan createdTrainingPlan)
    {
        try
        {
            var trainingPlanExercisesIds = new List<int>();

            foreach (WeeklyPlanWeekPostDto week in dto.WeeklyPlan)
            {
                trainingPlanExercisesIds.AddRange(this.GetExerciseIds(week.Days.Monday));
                trainingPlanExercisesIds.AddRange(this.GetExerciseIds(week.Days.Tuesday));
                trainingPlanExercisesIds.AddRange(this.GetExerciseIds(week.Days.Wednesday));
                trainingPlanExercisesIds.AddRange(this.GetExerciseIds(week.Days.Thursday));
                trainingPlanExercisesIds.AddRange(this.GetExerciseIds(week.Days.Friday));
                trainingPlanExercisesIds.AddRange(this.GetExerciseIds(week.Days.Saturday));
                trainingPlanExercisesIds.AddRange(this.GetExerciseIds(week.Days.Sunday));
            }

            trainingPlanExercisesIds = trainingPlanExercisesIds.Distinct().ToList();

            var exercises = await _dbContext.Exercises.Where(x => trainingPlanExercisesIds.Contains(x.Id)).ToListAsync();

            if (exercises.Count != trainingPlanExercisesIds.Count)
            {
                return Task.FromResult(false).Result;
            }

            var trainingPlanExercises = new List<TrainingPlanExercise>();

            foreach (var week in dto.WeeklyPlan)
            {
                var list = this.GetWeekTrainingPlanExercises(week, exercises, createdTrainingPlan);

                if (list is null) { return Task.FromResult(false).Result; }

                trainingPlanExercises.AddRange(list);
            }

            await _dbContext.AddRangeAsync(trainingPlanExercises);
            await _dbContext.SaveChangesAsync();

            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }

    }

    private IEnumerable<int> GetExerciseIds(List<ExercisesWithSetsPostDto> exercisesInfo)
    {
        if (exercisesInfo.IsNullOrEmpty())
        {
            return Enumerable.Empty<int>();
        }

        return exercisesInfo.Select(x => x.Id);
    }

    private List<TrainingPlanExercise>? GetWeekTrainingPlanExercises(WeeklyPlanWeekPostDto week, List<Exercise> exercises, TrainingPlan createdTrainingPlan)
    {
        try
        {
            var list = new List<TrainingPlanExercise>();

            list.AddRange(this.GetDayTrainingPlanExercises(week.Days.Monday, exercises, Days.Monday, week.Week, createdTrainingPlan));
            list.AddRange(this.GetDayTrainingPlanExercises(week.Days.Tuesday, exercises, Days.Tuesday, week.Week, createdTrainingPlan));
            list.AddRange(this.GetDayTrainingPlanExercises(week.Days.Wednesday, exercises, Days.Wednesday, week.Week, createdTrainingPlan));
            list.AddRange(this.GetDayTrainingPlanExercises(week.Days.Thursday, exercises, Days.Thursday, week.Week, createdTrainingPlan));
            list.AddRange(this.GetDayTrainingPlanExercises(week.Days.Friday, exercises, Days.Friday, week.Week, createdTrainingPlan));
            list.AddRange(this.GetDayTrainingPlanExercises(week.Days.Saturday, exercises, Days.Saturday, week.Week, createdTrainingPlan));
            list.AddRange(this.GetDayTrainingPlanExercises(week.Days.Sunday, exercises, Days.Sunday, week.Week, createdTrainingPlan));

            return list;
        }
        catch
        {
            return null;
        }
    }

    private List<TrainingPlanExercise> GetDayTrainingPlanExercises(List<ExercisesWithSetsPostDto> dayExercices, List<Exercise> exercises, Days dayType, int week, TrainingPlan createdTrainingPlan)
    {
        var list = new List<TrainingPlanExercise>();

        if (dayExercices.IsNullOrEmpty()) { return list; }

        foreach (var exercise in dayExercices)
        {
            list.Add(new TrainingPlanExercise
            {
                Exercise = exercises.First(x => x.Id == exercise.Id),
                Day = dayType,
                TrainingPlan = createdTrainingPlan,
                Week = week,
                Sets = exercise.Sets
            });
        }

        if (dayExercices.Count != list.Count)
        {
            throw new Exception();
        }

        return list;
    }
}
