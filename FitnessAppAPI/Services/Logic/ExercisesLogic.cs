using DataAccess.DatabaseContext;
using DataAccess.Enumerators;
using DataAccess.Models.SportsClubModels;
using DataAccess.Models.TrainingPlanModels;
using FitnessAppAPI.DTOs.Exercise;
using FitnessAppAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessAppAPI.Services.Logic;

public class ExercisesLogic : IExercisesLogic
{
    private readonly FitnessAppDbContext _dbContext;

    public ExercisesLogic(FitnessAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ExerciseGetDto>> GetTrainersExerices(int userId)
    {
        return Task.FromResult(_dbContext.Exercises.Include(x => x.Equipment).Where(x => x.CreatedBy.Id == userId)
            .Select(x => new ExerciseGetDto
            {
                CreatedBy = userId,
                Equipment = new Equipment
                {
                    Id = x.Equipment.Id,
                    Description = x.Equipment.Description,
                    ImageURI = x.Equipment.ImageURI,
                    Name = x.Equipment.Name
                }
            }).ToList()).Result;
    }

    public async Task<bool> CreateExercise(int userId, ExercisePostDto exerciseDto)
    {
        try
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return Task.FromResult(false).Result;
            }

            ExerciseTypes exerciseType;

            if (!Enum.TryParse<ExerciseTypes>(exerciseDto.ExerciseTypes, out exerciseType))
            {
                return Task.FromResult(false).Result;
            }

            var exercise = new Exercise
            {
                CreatedBy = user,
                ExerciseType = exerciseType,
                MuscleGroups = exerciseDto.MuscleGroups,
                Name = exerciseDto.Name,
            };

            if (exerciseDto.EquipmentId != null)
            {
                var equipment = await _dbContext.Equipment.FirstOrDefaultAsync(x => x.Id == exerciseDto.EquipmentId);
                exercise.Equipment = equipment;
            }

            var addedExercise = await _dbContext.Exercises.AddAsync(exercise);

            if (exerciseDto.Guide != null)
            {
                var exerciseGuide = new ExerciseGuide
                {
                    Exercise = addedExercise.Entity,
                    Guide = exerciseDto.Guide,
                };

                await _dbContext.ExerciseGuides.AddAsync(exerciseGuide);
            }

            await _dbContext.SaveChangesAsync();
            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }
}
