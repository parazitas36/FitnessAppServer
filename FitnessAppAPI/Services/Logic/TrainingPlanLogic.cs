namespace FitnessAppAPI.Services.Logic;

using DataAccess.DatabaseContext;
using DataAccess.Enumerators;
using DataAccess.Models.TrainingPlanModels;
using DataAccess.Models.UserModels;
using FitnessAppAPI.DTOs.Equipment;
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

    public async Task<bool> CopyTrainingPlan(int trainerId, int trainingPlanId, string coppiedTrainingPlanName)
    {
        var trainingPlan = await _dbContext.TrainingPlans.Include(x => x.CreatedBy).FirstOrDefaultAsync(x => x.CreatedBy.Id == trainerId && x.Id == trainingPlanId);

        if (trainingPlan == null) { return Task.FromResult(false).Result; }

        var trainingPlanExercises = await _dbContext.TrainingPlanExercises.Include(x => x.Exercise).Where(x => x.TrainingPlan.Id == trainingPlanId).ToListAsync();

        try
        {
            var newTrainingPlan = new TrainingPlan
            {
                CreatedBy = trainingPlan.CreatedBy,
                Name = coppiedTrainingPlanName
            };

            var insertedTrainingPlan = (await _dbContext.TrainingPlans.AddAsync(newTrainingPlan)).Entity;

            var newTrainingPlanExercises = new List<TrainingPlanExercise>();

            foreach (var trainingPlanExercise in trainingPlanExercises)
            {
                newTrainingPlanExercises.Add(new TrainingPlanExercise
                {
                    TrainingPlan = insertedTrainingPlan,
                    Day = trainingPlanExercise.Day,
                    Week = trainingPlanExercise.Week,
                    Sets = trainingPlanExercise.Sets,
                    Exercise = trainingPlanExercise.Exercise,
                });
            }

            await _dbContext.TrainingPlanExercises.AddRangeAsync(newTrainingPlanExercises);
            await _dbContext.SaveChangesAsync();
            return Task.FromResult(true).Result;
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
                .Include(x => x.TrainingPlan.CreatedBy)
                .Include(x => x.Exercise)
                .Include(x => x.Exercise.Equipment)
                .Where(x => x.TrainingPlan.CreatedBy.Id == trainerId).ToListAsync();

            var trainingPlans = new List<TrainingPlanShortGetDto>();
            var uniqueTrainingPlans = trainingPlanExercises.DistinctBy(x => x.TrainingPlan.Id).Select(x => x.TrainingPlan).ToList();
            var uniqueIds = uniqueTrainingPlans.Select(x => x.Id).ToList();

            var assignedTrainingPlans = await _dbContext.ClientTrainingPlans
                .Include(x => x.TrainingPlan)
                .Include(x => x.Client)
                .Where(x => uniqueIds.Contains(x.TrainingPlan.Id))
                .ToListAsync();

            foreach (var trainingPlan in uniqueTrainingPlans)
            {
                if (!trainingPlans.Any(x => x.Id == trainingPlan.Id))
                {
                    trainingPlans.Add(new TrainingPlanShortGetDto
                    {
                        Id = trainingPlan.Id,
                        CreatedById = trainerId,
                        CreatedBy = trainingPlan.CreatedBy.Username,
                        Equipment = trainingPlanExercises.Where(x => x.TrainingPlan.Id == trainingPlan.Id).Select(x => x.Exercise.Equipment).Where(x => x is not null).Distinct().ToList(),
                        MuscleGroups = trainingPlanExercises.Where(x => x.TrainingPlan.Id == trainingPlan.Id).Select(x => x.Exercise.MuscleGroups).Distinct().ToList(),
                        Name = trainingPlan.Name,
                        AssignedTo = assignedTrainingPlans.FirstOrDefault(x => x.TrainingPlan.Id == trainingPlan.Id)?.Client.Username
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

    public async Task<List<TrainingPlanShortGetDto>> GetUsersTrainingPlanShortList(int userId)
    {
        try
        {
            var trainingPlanExercises = await _dbContext.TrainingPlanExercises
                .Include(x => x.TrainingPlan)
                .Include(x => x.Exercise)
                .Include(x => x.Exercise.Equipment)
                .Include(x => x.TrainingPlan.CreatedBy)
                .Where(x => 
                    _dbContext.ClientTrainingPlans
                    .FirstOrDefault(y => y.TrainingPlan.Id == x.TrainingPlan.Id && y.Client.Id == userId).TrainingPlan.Id == x.TrainingPlan.Id)
                .ToListAsync();

            var trainingPlans = new List<TrainingPlanShortGetDto>();
            var uniqueTrainingPlans = trainingPlanExercises.DistinctBy(x => x.TrainingPlan.Id).Select(x => x.TrainingPlan).ToList();

            foreach (var trainingPlan in uniqueTrainingPlans)
            {
                if (!trainingPlans.Any(x => x.Id == trainingPlan.Id))
                {
                    trainingPlans.Add(new TrainingPlanShortGetDto
                    {
                        Id = trainingPlan.Id,
                        CreatedById = trainingPlan.CreatedBy.Id,
                        CreatedBy = trainingPlan.CreatedBy.Username,
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

    public async Task<List<TrainingPlanShortGetDto>> GetClientTrainingPlans(int trainerId, int clientId)
    {
        try
        {
            var trainingPlanExercises = await _dbContext.TrainingPlanExercises
                .Include(x => x.TrainingPlan)
                .Include(x => x.Exercise)
                .Include(x => x.Exercise.Equipment)
                .Include(x => x.TrainingPlan.CreatedBy)
                .Where(x => _dbContext.ClientTrainingPlans
                    .FirstOrDefault(y => y.TrainingPlan.Id == x.TrainingPlan.Id && y.Client.Id == clientId && y.TrainingPlan.CreatedBy.Id == trainerId)
                    .TrainingPlan.Id == x.TrainingPlan.Id)
                .ToListAsync();

            var trainingPlans = new List<TrainingPlanShortGetDto>();
            var uniqueTrainingPlans = trainingPlanExercises.DistinctBy(x => x.TrainingPlan.Id).Select(x => x.TrainingPlan).ToList();

            foreach (var trainingPlan in uniqueTrainingPlans)
            {
                if (!trainingPlans.Any(x => x.Id == trainingPlan.Id))
                {
                    trainingPlans.Add(new TrainingPlanShortGetDto
                    {
                        Id = trainingPlan.Id,
                        CreatedById = trainingPlan.CreatedBy.Id,
                        CreatedBy = trainingPlan.CreatedBy.Username,
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

    public async Task<UserTrainingPlanGetDto?> GetClientTrainingPlanById(int trainerId, int userId, int trainingPlanId)
    {
        var trainingPlan = await _dbContext.ClientTrainingPlans
            .FirstOrDefaultAsync(x => x.Client.Id == userId && x.TrainingPlan.Id == trainingPlanId && x.TrainingPlan.CreatedBy.Id == trainerId);

        if (trainingPlan == null) { return null; }

        return await GetUserTrainingPlanById(userId, trainingPlanId);
    }

    public async Task<UserTrainingPlanGetDto?> GetUserTrainingPlanById(int userId, int trainingPlanId)
    {
        var trainingPlan = await _dbContext.ClientTrainingPlans
            .Include(x => x.TrainingPlan)
            .FirstOrDefaultAsync(x => x.TrainingPlan.Id == trainingPlanId && x.Client.Id == userId);

        if (trainingPlan == null) { return null; }

        var weeklyPlan = _dbContext.TrainingPlanExercises
            .Where(x => x.TrainingPlan.Id == trainingPlanId)
            .Select(x => x.Week)
            .OrderBy(x => x)
            .Distinct()
            .Select(week => new UserWeeklyPlanWeekGetDto
            {
                Week = (int)week,
                Days = new UserWeeklyPlanDaysGetDto
                {
                    Monday = _dbContext.TrainingPlanExercises
                        .Where(tpe => tpe.Week == week && tpe.TrainingPlan.Id == trainingPlanId && tpe.Day == Days.Monday)
                        .Select(tpe => new UserExercisesWithSetsGetDto
                        {
                            ExerciseId = tpe.Exercise.Id,
                            ExerciseName = tpe.Exercise.Name,
                            MuscleGroups = tpe.Exercise.MuscleGroups,
                            Sets = tpe.Sets,
                            TrainingPlanExerciseId = tpe.Id,
                            Equipment = _dbContext.Equipment.Where(eq => eq.Id == tpe.Exercise.Equipment.Id).Select(eq => new EquipmentGetDto
                            {
                                Id = eq.Id,
                                Name = eq.Name,
                                ImageURI = eq.ImageURI
                            }).FirstOrDefault(),
                            LoggedSets = _dbContext.ExerciseProgress.FirstOrDefault(ls => ls.TrainingPlanExercise.Id == tpe.Id).LoggedSets,
                        }).ToList(),
                    Tuesday = _dbContext.TrainingPlanExercises
                        .Where(tpe => tpe.Week == week && tpe.TrainingPlan.Id == trainingPlanId && tpe.Day == Days.Tuesday)
                        .Select(tpe => new UserExercisesWithSetsGetDto
                        {
                            ExerciseId = tpe.Exercise.Id,
                            ExerciseName = tpe.Exercise.Name,
                            MuscleGroups = tpe.Exercise.MuscleGroups,
                            Sets = tpe.Sets,
                            TrainingPlanExerciseId = tpe.Id,
                            Equipment = _dbContext.Equipment.Where(eq => eq.Id == tpe.Exercise.Equipment.Id).Select(eq => new EquipmentGetDto
                            {
                                Id = eq.Id,
                                Name = eq.Name,
                                ImageURI = eq.ImageURI
                            }).FirstOrDefault(),
                            LoggedSets = _dbContext.ExerciseProgress.FirstOrDefault(ls => ls.TrainingPlanExercise.Id == tpe.Id).LoggedSets,
                        }).ToList(),
                    Wednesday = _dbContext.TrainingPlanExercises
                        .Where(tpe => tpe.Week == week && tpe.TrainingPlan.Id == trainingPlanId && tpe.Day == Days.Wednesday)
                        .Select(tpe => new UserExercisesWithSetsGetDto
                        {
                            ExerciseId = tpe.Exercise.Id,
                            ExerciseName = tpe.Exercise.Name,
                            MuscleGroups = tpe.Exercise.MuscleGroups,
                            Sets = tpe.Sets,
                            TrainingPlanExerciseId = tpe.Id,
                            Equipment = _dbContext.Equipment.Where(eq => eq.Id == tpe.Exercise.Equipment.Id).Select(eq => new EquipmentGetDto
                            {
                                Id = eq.Id,
                                Name = eq.Name,
                                ImageURI = eq.ImageURI
                            }).FirstOrDefault(),
                            LoggedSets = _dbContext.ExerciseProgress.FirstOrDefault(ls => ls.TrainingPlanExercise.Id == tpe.Id).LoggedSets,
                        }).ToList(),
                    Thursday = _dbContext.TrainingPlanExercises
                        .Where(tpe => tpe.Week == week && tpe.TrainingPlan.Id == trainingPlanId && tpe.Day == Days.Thursday)
                        .Select(tpe => new UserExercisesWithSetsGetDto
                        {
                            ExerciseId = tpe.Exercise.Id,
                            ExerciseName = tpe.Exercise.Name,
                            MuscleGroups = tpe.Exercise.MuscleGroups,
                            Sets = tpe.Sets,
                            TrainingPlanExerciseId = tpe.Id,
                            Equipment = _dbContext.Equipment.Where(eq => eq.Id == tpe.Exercise.Equipment.Id).Select(eq => new EquipmentGetDto
                            {
                                Id = eq.Id,
                                Name = eq.Name,
                                ImageURI = eq.ImageURI
                            }).FirstOrDefault(),
                            LoggedSets = _dbContext.ExerciseProgress.FirstOrDefault(ls => ls.TrainingPlanExercise.Id == tpe.Id).LoggedSets,
                        }).ToList(),
                    Friday = _dbContext.TrainingPlanExercises
                        .Where(tpe => tpe.Week == week && tpe.TrainingPlan.Id == trainingPlanId && tpe.Day == Days.Friday)
                        .Select(tpe => new UserExercisesWithSetsGetDto
                        {
                            ExerciseId = tpe.Exercise.Id,
                            ExerciseName = tpe.Exercise.Name,
                            MuscleGroups = tpe.Exercise.MuscleGroups,
                            Sets = tpe.Sets,
                            TrainingPlanExerciseId = tpe.Id,
                            Equipment = _dbContext.Equipment.Where(eq => eq.Id == tpe.Exercise.Equipment.Id).Select(eq => new EquipmentGetDto
                            {
                                Id = eq.Id,
                                Name = eq.Name,
                                ImageURI = eq.ImageURI
                            }).FirstOrDefault(),
                            LoggedSets = _dbContext.ExerciseProgress.FirstOrDefault(ls => ls.TrainingPlanExercise.Id == tpe.Id).LoggedSets,
                        }).ToList(),
                    Saturday = _dbContext.TrainingPlanExercises
                        .Where(tpe => tpe.Week == week && tpe.TrainingPlan.Id == trainingPlanId && tpe.Day == Days.Saturday)
                        .Select(tpe => new UserExercisesWithSetsGetDto
                        {
                            ExerciseId = tpe.Exercise.Id,
                            ExerciseName = tpe.Exercise.Name,
                            MuscleGroups = tpe.Exercise.MuscleGroups,
                            Sets = tpe.Sets,
                            TrainingPlanExerciseId = tpe.Id,
                            Equipment = _dbContext.Equipment.Where(eq => eq.Id == tpe.Exercise.Equipment.Id).Select(eq => new EquipmentGetDto
                            {
                                Id = eq.Id,
                                Name = eq.Name,
                                ImageURI = eq.ImageURI
                            }).FirstOrDefault(),
                            LoggedSets = _dbContext.ExerciseProgress.FirstOrDefault(ls => ls.TrainingPlanExercise.Id == tpe.Id).LoggedSets,
                        }).ToList(),
                    Sunday = _dbContext.TrainingPlanExercises
                        .Where(tpe => tpe.Week == week && tpe.TrainingPlan.Id == trainingPlanId && tpe.Day == Days.Sunday)
                        .Select(tpe => new UserExercisesWithSetsGetDto
                        {
                            ExerciseId = tpe.Exercise.Id,
                            ExerciseName = tpe.Exercise.Name,
                            MuscleGroups = tpe.Exercise.MuscleGroups,
                            Sets = tpe.Sets,
                            TrainingPlanExerciseId = tpe.Id,
                            Equipment = _dbContext.Equipment.Where(eq => eq.Id == tpe.Exercise.Equipment.Id).Select(eq => new EquipmentGetDto
                            {
                                Id = eq.Id,
                                Name = eq.Name,
                                ImageURI = eq.ImageURI
                            }).FirstOrDefault(),
                            LoggedSets = _dbContext.ExerciseProgress.FirstOrDefault(ls => ls.TrainingPlanExercise.Id == tpe.Id).LoggedSets,
                        }).ToList(),
                }
            }).ToList();

        var result = new UserTrainingPlanGetDto
        {
            Name = trainingPlan.TrainingPlan.Name,
            TrainingPlanId = trainingPlanId,
            WeeklyPlan = weeklyPlan
        };

        return Task.FromResult(result).Result;
    }

    public async Task<TrainingPlanGetDto?> GetTrainingPlanById(int userId, int trainingPlanId)
    {
        List<TrainingPlanExercise> trainingPlanExercises = await _dbContext.TrainingPlanExercises.Include(x => x.TrainingPlan).Include(x => x.Exercise)
            .Where(x => x.TrainingPlan.CreatedBy.Id == userId && x.TrainingPlan.Id == trainingPlanId).ToListAsync();

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

        int editKey = 1;

        foreach (var week in weeks)
        {
            weeklyPlan.Add(new WeeklyPlanWeekGetDto
            {
                Week = (int)week,
                Days = new WeeklyPlanDaysGetDto
                {
                    Monday = trainingPlanExercises.Where(x => x.Week == week && x.Day == Days.Monday).Select(x => new ExercisesWithSetsGetDto
                    {
                        EditKey = editKey++,
                        TrainingPlanExerciseId = x.Id,
                        ExerciseId = x.Exercise.Id,
                        Sets = x.Sets
                    }).ToList(),

                    Tuesday = trainingPlanExercises.Where(x => x.Week == week && x.Day == Days.Tuesday).Select(x => new ExercisesWithSetsGetDto
                    {
                        EditKey = editKey++,
                        TrainingPlanExerciseId = x.Id,
                        ExerciseId = x.Exercise.Id,
                        Sets = x.Sets
                    }).ToList(),

                    Wednesday = trainingPlanExercises.Where(x => x.Week == week && x.Day == Days.Wednesday).Select(x => new ExercisesWithSetsGetDto
                    {
                        EditKey = editKey++,
                        TrainingPlanExerciseId = x.Id,
                        ExerciseId = x.Exercise.Id,
                        Sets = x.Sets
                    }).ToList(),

                    Thursday = trainingPlanExercises.Where(x => x.Week == week && x.Day == Days.Thursday).Select(x => new ExercisesWithSetsGetDto
                    {
                        EditKey = editKey++,
                        TrainingPlanExerciseId = x.Id,
                        ExerciseId = x.Exercise.Id,
                        Sets = x.Sets
                    }).ToList(),

                    Friday = trainingPlanExercises.Where(x => x.Week == week && x.Day == Days.Friday).Select(x => new ExercisesWithSetsGetDto
                    {
                        EditKey = editKey++,
                        TrainingPlanExerciseId = x.Id,
                        ExerciseId = x.Exercise.Id,
                        Sets = x.Sets
                    }).ToList(),

                    Saturday = trainingPlanExercises.Where(x => x.Week == week && x.Day == Days.Saturday).Select(x => new ExercisesWithSetsGetDto
                    {
                        EditKey = editKey++,
                        TrainingPlanExerciseId = x.Id,
                        ExerciseId = x.Exercise.Id,
                        Sets = x.Sets
                    }).ToList(),

                    Sunday = trainingPlanExercises.Where(x => x.Week == week && x.Day == Days.Sunday).Select(x => new ExercisesWithSetsGetDto
                    {
                        EditKey = editKey++,
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

    public async Task<bool> AssignTrainingPlan(int trainerId, int clientId, int trainingPlanId)
    {
        var alreadyAssigned = await _dbContext.ClientTrainingPlans.FirstOrDefaultAsync(x => x.TrainingPlan.Id == trainingPlanId);

        if (alreadyAssigned != null) { return Task.FromResult(false).Result; }

        var client = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == clientId);

        if (client == null) { return Task.FromResult(false).Result; }

        var trainingPlan = await _dbContext.TrainingPlans.FirstOrDefaultAsync(x => x.CreatedBy.Id == trainerId && x.Id == trainingPlanId);

        if (trainingPlan == null) { return Task.FromResult(false).Result; }

        try
        {
            var clientTrainingPlan = new ClientTrainingPlan
            {
                Client = client,
                TrainingPlan = trainingPlan
            };

            await _dbContext.ClientTrainingPlans.AddAsync(clientTrainingPlan);
            await _dbContext.SaveChangesAsync();

            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }

    public async Task<bool> LogExerciseSet(int userId, int trainingPlanExerciseId, string loggedSets)
    {
        var trainingPlanExercise = await _dbContext.TrainingPlanExercises
            .Where(x => x.Id == trainingPlanExerciseId && _dbContext.ClientTrainingPlans
                    .FirstOrDefault(y => y.TrainingPlan.Id == x.TrainingPlan.Id && y.Client.Id == userId) != null)
            .FirstOrDefaultAsync();

        if (trainingPlanExercise == null) { return Task.FromResult(false).Result; }

        try
        {
            var existingProgress = await _dbContext.ExerciseProgress.FirstOrDefaultAsync(x => x.TrainingPlanExercise.Id == trainingPlanExerciseId);

            if (existingProgress != null)
            {
                existingProgress.LoggedSets = loggedSets;
                _dbContext.ExerciseProgress.Update(existingProgress);
            }
            else
            {
                var newExerciseProgress = new ExerciseProgress
                {
                    LoggedSets = loggedSets,
                    TrainingPlanExercise = trainingPlanExercise
                };

                await _dbContext.ExerciseProgress.AddAsync(newExerciseProgress);
            }

            await _dbContext.SaveChangesAsync();
            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }

    public async Task<bool> UpdateTrainingPlanAddNewExercise(int trainerId, int trainingPlanId, TrainingPlanNewExerciseUpdateDto dto)
    {
        var trainingPlan = await _dbContext.TrainingPlans.FirstOrDefaultAsync(x => x.CreatedBy.Id == trainerId && x.Id == trainingPlanId);
        var exercise = await _dbContext.Exercises.FirstOrDefaultAsync(x => x.Id == dto.ExerciseId && x.CreatedBy.Id == trainerId);

        Days day;

        if (trainingPlan == null || exercise == null || !Enum.TryParse(dto.Day, out day)) { return Task.FromResult(false).Result; }

        try
        {
            var newTrainingPlanExercise = new TrainingPlanExercise
            {
                Day = day,
                Week = dto.Week,
                Exercise = exercise,
                Sets = dto.Sets,
                TrainingPlan = trainingPlan
            };

            await _dbContext.TrainingPlanExercises.AddAsync(newTrainingPlanExercise);
            await _dbContext.SaveChangesAsync();
            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }

    public async Task<bool> UpdateTrainingPlanExercise(int trainerId, int trainingPlanExerciseId, string sets)
    {
        var trainingPlanExercise = await _dbContext.TrainingPlanExercises
            .FirstOrDefaultAsync(x => x.TrainingPlan.CreatedBy.Id == trainerId && x.Id == trainingPlanExerciseId);

        if (trainingPlanExercise == null) { return Task.FromResult(false).Result; }

        try
        {
            trainingPlanExercise.Sets = sets;

            _dbContext.TrainingPlanExercises.Update(trainingPlanExercise);
            await _dbContext.SaveChangesAsync();
            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }

    public async Task<bool> DeleteTrainingPlanExercise(int trainerId, int trainingPlanExerciseId)
    {
        var clientProgress = await _dbContext.ExerciseProgress.FirstOrDefaultAsync(x => x.TrainingPlanExercise.Id == trainingPlanExerciseId);

        if (clientProgress != null) { return Task.FromResult(false).Result; }

        var trainingPlanExercise = await _dbContext.TrainingPlanExercises.FirstOrDefaultAsync(x => x.Id == trainingPlanExerciseId && x.TrainingPlan.CreatedBy.Id == trainerId);

        if(trainingPlanExercise == null) { return Task.FromResult(false).Result; }

        try
        {
            _dbContext.TrainingPlanExercises.Remove(trainingPlanExercise);
            await _dbContext.SaveChangesAsync();

            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }
}
