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

            var createdTrainingPlan = (await _dbContext.TrainingPlans.AddAsync(new TrainingPlan
            {
                CreatedBy = user,
                Name = dto.Name,
            })).Entity;

            return Task.FromResult(await this.ProcessWeeklyTrainingPlan(dto, createdTrainingPlan)).Result;
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
            var trainingPlanExercises = await _dbContext.TrainingPlanExercises
                .Include(x => x.TrainingPlan)
                .Include(x => x.Exercise)
                .Include(x => x.Exercise.Equipment)
                .Where(x => x.TrainingPlan.CreatedBy.Id == trainerId).ToListAsync();

            var trainingPlans = new List<TrainingPlanShortGetDto>();
            var uniqueTrainingPlans = trainingPlanExercises.DistinctBy(x => x.TrainingPlan.Id).Select(x => x.TrainingPlan).ToList();

            foreach (var trainingPlan in uniqueTrainingPlans)
            {
                if (!trainingPlans.Any(x => x.Id == trainingPlan.Id))
                {
                    trainingPlans.Add(new TrainingPlanShortGetDto
                    {
                        Id = trainingPlan.Id,
                        CreatedById = trainerId,
                        Equipment = trainingPlanExercises.Where(x => x.TrainingPlan.Id == trainingPlan.Id).Select(x => x.Exercise.Equipment).Where(x => x is not null).Distinct().ToList(),
                        MuscleGroups = trainingPlanExercises.Where(x => x.TrainingPlan.Id == trainingPlan.Id).Select(x => x.Exercise.MuscleGroups).Distinct().ToList(),
                        Name = trainingPlan.Name,
                    });
                }
            }

            return Task.FromResult(trainingPlans).Result;
        }
        catch
        {
            return null;
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

    public async Task<TrainingPlanGetDto?> GetTrainingPlanById(int userId, int trainingPlanId, bool trainer)
    {
        if (trainer == false)
        {
            var result = _dbContext.ClientTrainingPlans.FirstOrDefaultAsync(x => x.TrainingPlan.Id == trainingPlanId && x.Client.Id == userId);

            if (result is null) { return null; }
        }

        List<TrainingPlanExercise> trainingPlanExercises = null;

        if (trainer)
        {
            trainingPlanExercises = await _dbContext.TrainingPlanExercises.Include(x => x.TrainingPlan).Include(x => x.Exercise)
                .Where(x => x.TrainingPlan.CreatedBy.Id == userId && x.TrainingPlan.Id == trainingPlanId).ToListAsync();
        }
        else
        {
            trainingPlanExercises = await _dbContext.TrainingPlanExercises.Include(x => x.TrainingPlan).Include(x => x.Exercise)
                .Where(x => x.TrainingPlan.Id == trainingPlanId).ToListAsync();
        }

        if (trainingPlanExercises.IsNullOrEmpty()) { return null; }

        List<int?> weeks = trainingPlanExercises.Select(x => x.Week).Distinct().ToList();

        for (int i = 1; i < weeks.Last(); i++)
        {
            if (!weeks.Contains(i))
            {
                weeks.Add(i);
            }
        }

        weeks = weeks.OrderBy(x => x).ToList();

        List<WeeklyPlanWeekGetDto> weeklyPlan = new List<WeeklyPlanWeekGetDto>();

        foreach (var week in weeks)
        {
            weeklyPlan.Add(new WeeklyPlanWeekGetDto
            {
                Week = (int)week,
                Days = new WeeklyPlanDaysGetDto
                {
                    Monday = trainingPlanExercises.Where(x => x.Week == week && x.Day == Days.Monday).Select(x => new ExercisesWithSetsGetDto
                    {
                        TrainingPlanExerciseId = x.Id,
                        ExerciseId = x.Exercise.Id,
                        Sets = x.Sets
                    }).ToList(),

                    Tuesday = trainingPlanExercises.Where(x => x.Week == week && x.Day == Days.Tuesday).Select(x => new ExercisesWithSetsGetDto
                    {
                        TrainingPlanExerciseId = x.Id,
                        ExerciseId = x.Exercise.Id,
                        Sets = x.Sets
                    }).ToList(),

                    Wednesday = trainingPlanExercises.Where(x => x.Week == week && x.Day == Days.Wednesday).Select(x => new ExercisesWithSetsGetDto
                    {
                        TrainingPlanExerciseId = x.Id,
                        ExerciseId = x.Exercise.Id,
                        Sets = x.Sets
                    }).ToList(),

                    Thursday = trainingPlanExercises.Where(x => x.Week == week && x.Day == Days.Thursday).Select(x => new ExercisesWithSetsGetDto
                    {
                        TrainingPlanExerciseId = x.Id,
                        ExerciseId = x.Exercise.Id,
                        Sets = x.Sets
                    }).ToList(),

                    Friday = trainingPlanExercises.Where(x => x.Week == week && x.Day == Days.Friday).Select(x => new ExercisesWithSetsGetDto
                    {
                        TrainingPlanExerciseId = x.Id,
                        ExerciseId = x.Exercise.Id,
                        Sets = x.Sets
                    }).ToList(),

                    Saturday = trainingPlanExercises.Where(x => x.Week == week && x.Day == Days.Saturday).Select(x => new ExercisesWithSetsGetDto
                    {
                        TrainingPlanExerciseId = x.Id,
                        ExerciseId = x.Exercise.Id,
                        Sets = x.Sets
                    }).ToList(),

                    Sunday = trainingPlanExercises.Where(x => x.Week == week && x.Day == Days.Sunday).Select(x => new ExercisesWithSetsGetDto
                    {
                        TrainingPlanExerciseId = x.Id,
                        ExerciseId = x.Exercise.Id,
                        Sets = x.Sets
                    }).ToList(),
                },
            });
        }

        return Task.FromResult(new TrainingPlanGetDto
        {
            Name = trainingPlanExercises.FirstOrDefault()?.TrainingPlan.Name,
            TrainingPlanId = trainingPlanId,
            WeeklyPlan = weeklyPlan
        }).Result;
    }
}
