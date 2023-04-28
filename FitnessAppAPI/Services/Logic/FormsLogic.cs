namespace FitnessAppAPI.Services.Logic;

using DataAccess.DatabaseContext;
using DataAccess.Models.FormsModels;
using DataAccess.Models.UserModels;
using FitnessAppAPI.DTOs.Forms;
using FitnessAppAPI.Services.Helpers;
using FitnessAppAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class FormsLogic : IFormsLogic
{
    private readonly FitnessAppDbContext _dbContext;

    public FormsLogic(FitnessAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> PostMeasurements(int userId, BodyMeasurementsPostDto dto)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null) { return Task.FromResult(false).Result; }

        try
        {
            var imageUri = await FilesHandler.SaveFile(dto.Image);

            var bodyMeasurements = new BodyMeasurements
            {
                User = user,
                Bust = dto.Bust,
                Weight = dto.Weight,
                Hip = dto.Hip,
                ImperialSystem = user.UsesImperialSystem,
                Waist = dto.Waist,
                MeasurementDay = DateTime.Now,
                ImageUri = imageUri,
            };

            await _dbContext.BodyMeasurements.AddAsync(bodyMeasurements);
            await _dbContext.SaveChangesAsync();
            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }

    public async Task<bool> PostTrainingPlanForm(int userId, TrainingPlanFormPostDto dto)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null) { return Task.FromResult(false).Result; }

        try
        {
            var trainingPlanForm = new TrainingPlanForm
            {
                FormDetails = dto.FormDetails,
                CreatedBy = user,
            };

            await _dbContext.TrainingPlanForms.AddAsync(trainingPlanForm);
            await _dbContext.SaveChangesAsync();
            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }

    public async Task<bool> PostTrainerJobForm(int trainerId, TrainerJobFormPostDto dto)
    {
        var trainer = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == trainerId);

        if (trainer == null) { return Task<bool>.FromResult(false).Result; }

        try
        {
            var trainerJobFomr = new TrainerJobForm
            {
                City = dto.City,
                Country = dto.Country,
                CreationDate = DateTime.Now,
                OtherDetails = dto.OtherDetails,
                Education = dto.Education,
                PersonalAchievements = dto.PersonalAchievements,
                Trainer = trainer,
            };

            await _dbContext.TrainerJobForms.AddRangeAsync(trainerJobFomr);
            await _dbContext.SaveChangesAsync();
            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }

    public async Task<bool> PostJobOffer(int sportsClubAdminId, JobOfferPostDto dto)
    {
        var sportsClub = await _dbContext.SportsClubs.FirstOrDefaultAsync(x => x.Owner.Id == sportsClubAdminId);
        var trainerJobForm = await _dbContext.TrainerJobForms.FirstOrDefaultAsync(x => x.Id == dto.TrainerJobFormId);

        if (sportsClub == null || trainerJobForm == null) { return Task.FromResult(false).Result; }

        try
        {
            var jobOffer = new JobOffer
            {
                TrainerJobForm = trainerJobForm,
                Details = dto.Details,
                OfferDate = DateTime.Now,
                SportsClub = sportsClub,
                Status = DataAccess.Enumerators.OfferStatus.Offered
            };

            await _dbContext.JobOffers.AddAsync(jobOffer);
            await _dbContext.SaveChangesAsync();
            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }

    public async Task<List<BodyMeasurementsGetDto>> GetBodyMeasurements(int userId)
    {
        var result = await _dbContext.BodyMeasurements.Where(x => x.User.Id == userId).Select(x => new BodyMeasurementsGetDto
        {
            Id = x.Id,
            MeasurementDay = x.MeasurementDay,
            Bust = x.Bust,
            Hip = x.Hip,
            UserId = userId,
            Waist = x.Waist,
            Weight = x.Weight,
            ImageUri = x.ImageUri
        }).ToListAsync();

        return Task.FromResult(result).Result;
    }

    public async Task<List<JobOfferGetDto>> GetJobOffers(int trainerId)
    {
        var result = await _dbContext.JobOffers.Include(x => x.SportsClub).Include(x => x.TrainerJobForm)
            .Where(x => x.TrainerJobForm.Trainer.Id == trainerId).Select(x => new JobOfferGetDto
            {
                TrainerJobFormId = x.TrainerJobForm.Id,
                Details = x.Details,
                OfferDate = x.OfferDate,
                SportsClubId = x.SportsClub.Id,
                SportsClubName = x.SportsClub.Name,
                Status = x.Status.ToString(),
                Id = x.Id,
            }).ToListAsync();

        return Task.FromResult(result).Result;
    }

    public async Task<List<JobOfferGetDto>> GetSportsClubsJobOffers(int sportsClubAdminId)
    {
        var result = await _dbContext.JobOffers.Include(x => x.SportsClub).Include(x => x.TrainerJobForm)
            .Include(x => x.TrainerJobForm.Trainer)
            .Where(x => x.SportsClub.Owner.Id == sportsClubAdminId).Select(x => new JobOfferGetDto
            {
                TrainerId = x.TrainerJobForm.Trainer.Id,
                TrainerUserName = x .TrainerJobForm.Trainer.Username,
                TrainerJobFormId = x.TrainerJobForm.Id,
                Details = x.Details,
                OfferDate = x.OfferDate,
                SportsClubId = x.SportsClub.Id,
                SportsClubName = x.SportsClub.Name,
                Status = x.Status.ToString(),
                Id = x.Id,
            }).ToListAsync();

        return Task.FromResult(result).Result;
    }

    public async Task<List<TrainerJobFormGetDto>> GetTrainerJobForms(int trainerId)
    {
        var result = await _dbContext.TrainerJobForms.Include(x => x.Trainer)
            .Where(x => x.Trainer.Id == trainerId).Select(x => new TrainerJobFormGetDto
            {
                City = x.City,
                CreationDate = x.CreationDate,
                OtherDetails = x.OtherDetails,
                Country = x.Country,
                Education = x.Education,
                Id = x.Id,
                PersonalAchievements = x.PersonalAchievements,
                TrainerId = trainerId,
                TrainerUsername = x.Trainer.Username,
            }).ToListAsync();

        return Task.FromResult(result).Result;
    }

    public async Task<List<TrainerJobFormGetDto>> GetTrainersJobForms()
    {
        var result = await _dbContext.TrainerJobForms.Include(x => x.Trainer)
            .Select(x => new TrainerJobFormGetDto
            {
                City = x.City,
                CreationDate = x.CreationDate,
                OtherDetails = x.OtherDetails,
                Country = x.Country,
                Education = x.Education,
                Id = x.Id,
                PersonalAchievements = x.PersonalAchievements,
                TrainerId = x.Trainer.Id,
                TrainerUsername = x.Trainer.Username
            }).ToListAsync();

        return Task.FromResult(result).Result;
    }

    public async Task<List<TrainingPlanFormGetDto>> GetUsersTrainingPlanForms(int userId)
    {
        var result = await _dbContext.TrainingPlanForms.Include(x => x.CreatedBy)
            .Where(x => x.CreatedBy.Id == userId)?
            .Select(x => new TrainingPlanFormGetDto
            {
                Id = x.Id,
                FormDetails = x.FormDetails,
                UserId = userId,
                Username = x.CreatedBy.Username
            }).ToListAsync();

        return Task.FromResult(result).Result;
    }

    public async Task<List<TrainingPlanFormGetDto>> GetTrainingPlanForms()
    {
        var result = await _dbContext.TrainingPlanForms
            .Include(x => x.CreatedBy)
            .Select(x => new TrainingPlanFormGetDto
            {
                Id = x.Id,
                FormDetails = x.FormDetails,
                UserId = x.CreatedBy.Id,
                Username = x.CreatedBy.Username
            }).ToListAsync();

        return Task.FromResult(result).Result;
    }
}
