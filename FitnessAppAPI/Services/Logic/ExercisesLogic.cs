using DataAccess.DatabaseContext;
using DataAccess.Enumerators;
using DataAccess.Models.SportsClubModels;
using DataAccess.Models.TrainingPlanModels;
using DataAccess.Models.UserModels;
using FitnessAppAPI.DTOs.Exercise;
using FitnessAppAPI.Models;
using FitnessAppAPI.Services.Helpers;
using FitnessAppAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace FitnessAppAPI.Services.Logic;

public class ExercisesLogic : IExercisesLogic
{
    private readonly FitnessAppDbContext _dbContext;

    public ExercisesLogic(FitnessAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ExerciseWithGuideGetDto> GetExerciseById(int exerciseId)
    {
        try
        {
            var query = await (from exercise in _dbContext.Exercises.Where(x => x.Id == exerciseId)
                               from exerciseGuide in _dbContext.ExerciseGuides.Where(x => x.Exercise.Id == exercise.Id).DefaultIfEmpty()
                               from equipment in _dbContext.Equipment.Where(x => exercise.Equipment.Id == x.Id).DefaultIfEmpty()
                               from user in _dbContext.Users.Where(x => x.Id == exercise.CreatedBy.Id)
                               select new ExerciseWithGuideGetDto
                               {
                                   CreatedBy = user.Id,
                                   Equipment = equipment != null ? new Equipment
                                   {
                                       Id = equipment.Id,
                                       Description = equipment.Description,
                                       ImageURI = equipment.ImageURI,
                                       Name = equipment.Name
                                   } : null,
                                   Name = exercise.Name,
                                   ExerciseType = exercise.ExerciseType.ToString(),
                                   Id = exercise.Id,
                                   MuscleGroups = exercise.MuscleGroups,
                                   Guide = string.IsNullOrEmpty(exerciseGuide.Guide) ? null : exerciseGuide.Guide
                               }).FirstOrDefaultAsync();
            return Task.FromResult(query).Result;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<ExerciseGetDto>> GetTrainersExerices(int userId)
    {
        try
        {

            var query = await (from exercise in _dbContext.Exercises.Where(x => x.CreatedBy.Id == userId)
                               from exerciseGuide in _dbContext.ExerciseGuides.Where(x => x.Exercise.Id == exercise.Id).DefaultIfEmpty()
                               from equipment in _dbContext.Equipment.Where(x => exercise.Equipment.Id == x.Id).DefaultIfEmpty()
                               select new ExerciseGetDto
                               {
                                   CreatedBy = userId,
                                   Equipment = equipment != null ? new Equipment
                                   {
                                       Id = equipment.Id,
                                       Description = equipment.Description,
                                       ImageURI = equipment.ImageURI,
                                       Name = equipment.Name
                                   } : null,
                                   Name = exercise.Name,
                                   ExerciseType = exercise.ExerciseType.ToString(),
                                   Id = exercise.Id,
                                   MuscleGroups = exercise.MuscleGroups,
                                   HasGuide = !string.IsNullOrEmpty(exerciseGuide.Guide)
                               }).ToListAsync();

            return Task.FromResult(query).Result;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CreateExercise(int userId, ExerciseWithGuidePostDto exerciseDto)
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

            if (!exerciseDto.BlockTypes.IsNullOrEmpty())
            {

                var guide = await this.GetExerciseGuideInJson(exerciseDto);

                var exerciseGuide = new ExerciseGuide
                {
                    Exercise = addedExercise.Entity,
                    Guide = guide,
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

    private async Task<List<string>> StoreFiles(List<IFormFile> files)
    {
        List<string> filePaths = new List<string>();
        
        foreach (var file in files)
        {
            filePaths.Add(await FilesHandler.SaveFile(file));
        }

        return Task.FromResult(filePaths).Result;
    }

    private async Task<string> GetExerciseGuideInJson(ExerciseWithGuidePostDto exerciseDto)
    {
        var filePaths = exerciseDto.Files.IsNullOrEmpty() ? null : await this.StoreFiles(exerciseDto.Files);

        var blocks = new List<ExerciseGuideBlock>();
        int fileNum = 0;
        int textNum = 0;

        foreach (var blockType in exerciseDto.BlockTypes)
        {
            if (Enum.TryParse(blockType, out BlockTypes blockTypeEnum))
            {
                switch (blockTypeEnum)
                {
                    case BlockTypes.ImageBlock:
                        blocks.Add(new ExerciseGuideBlock
                        {
                            Type = blockType,
                            Content = filePaths[fileNum++]
                        });
                        break;

                    case BlockTypes.TextBlock:
                        blocks.Add(new ExerciseGuideBlock
                        {
                            Type = blockType,
                            Content = exerciseDto.Texts[textNum++]
                        });
                        break;

                    case BlockTypes.VideoBlock:
                        blocks.Add(new ExerciseGuideBlock
                        {
                            Type = blockType,
                            Content = filePaths[fileNum++]
                        });
                        break;
                }
            }
        }

        
        var json = JsonSerializer.Serialize(blocks);
        return Task.FromResult(json).Result;
    }
}
