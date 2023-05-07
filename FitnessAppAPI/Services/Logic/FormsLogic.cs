namespace FitnessAppAPI.Services.Logic;

using DataAccess.DatabaseContext;
using DataAccess.Enumerators;
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
            string? imageUri = null;
            
            if (dto.Image != null)
            {
                imageUri = await FilesHandler.SaveFile(dto.Image);
            }

            var bodyMeasurements = new BodyMeasurements
            {
                User = user,
                Chest = dto.Chest,
                Shoulders = dto.Shoulders,
                Height = dto.Height,
                Weight = dto.Weight,
                Hips = dto.Hips,
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
            var existingTrainingPlanForm = await _dbContext.TrainingPlanForms.FirstOrDefaultAsync(x => x.CreatedBy.Id == userId);

            if (existingTrainingPlanForm != null)
            {
                existingTrainingPlanForm.FormDetails = dto.FormDetails;
                _dbContext.TrainingPlanForms.Update(existingTrainingPlanForm);
            }
            else
            {
                var trainingPlanForm = new TrainingPlanForm
                {
                    FormDetails = dto.FormDetails,
                    CreatedBy = user,
                };

                await _dbContext.TrainingPlanForms.AddAsync(trainingPlanForm);
            }
            
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
            Chest = x.Chest,
            Hips = x.Hips,
            Height = x.Height,
            Shoulders = x.Shoulders,
            UserId = userId,
            Waist = x.Waist,
            Weight = x.Weight,
            ImageUri = x.ImageUri
        }).OrderByDescending(x => x.MeasurementDay).ToListAsync();

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

    public async Task<List<TrainingPlanFormGetDto>> GetTrainingPlanForms(int trainerId)
    {
        var result = await _dbContext.TrainingPlanForms
            .Include(x => x.CreatedBy)
            .Select(x => new TrainingPlanFormGetDto
            {
                Id = x.Id,
                FormDetails = x.FormDetails,
                UserId = x.CreatedBy.Id,
                Username = x.CreatedBy.Username,
                Offered = _dbContext.TrainingPlanOffers.Where(y => y.Trainer.Id == trainerId && y.TrainingPlanForm.Id == x.Id).Count() > 0
            }).ToListAsync();

        return Task.FromResult(result).Result;
    }

    public async Task<List<TrainingPlanOfferGetDto>> GetTrainingPlanOffers(int userId, bool trainer = false)
    {
        var list = await _dbContext.TrainingPlanOffers
            .Include(x => x.Trainer)
            .Include(x => x.TrainingPlanForm)
            .Include(x => x.TrainingPlanForm.CreatedBy)
            .Where(x => (!trainer && x.TrainingPlanForm.CreatedBy.Id == userId && x.Status != OfferStatus.Declined) || (trainer && x.Trainer.Id == userId))
            .Select(x => new TrainingPlanOfferGetDto
            {
                PriceFrom = x.PriceFrom,
                PriceTo = x.PriceTo,
                Details = x.Details,
                Status = x.Status.ToString(),
                Trainer = new DTOs.User.TrainerGetDto
                {
                    Id = x.Trainer.Id,
                    Name = x.Trainer.Name,
                    LastName = x.Trainer.Surname,
                    Email = _dbContext.ContactInfo.FirstOrDefault(y => y.Id == x.Trainer.ContactInfo.Id).EmailAddress,
                    Phone = _dbContext.ContactInfo.FirstOrDefault(y => y.Id == x.Trainer.ContactInfo.Id).PhoneNumber,
                    Username = x.Trainer.Username,
                },
                CreatedBy = new DTOs.User.UserGetDto
                {
                    Id = x.TrainingPlanForm.CreatedBy.Id,
                    Username = x.TrainingPlanForm.CreatedBy.Username,
                    Name = x.TrainingPlanForm.CreatedBy.Name,
                    Surname = x.TrainingPlanForm.CreatedBy.Surname,
                },
                TrainingPlanOfferId = x.Id
            }).ToListAsync();

        return Task.FromResult(list).Result;
    }

    public async Task<bool> UpdateTrainingPlanOffer(int userId, int trainingPlanId, OfferStatus newStatus)
    {
        try
        {
            var trainingPlanOffer = await _dbContext.TrainingPlanOffers.FirstOrDefaultAsync(x => x.Id == trainingPlanId && x.TrainingPlanForm.CreatedBy.Id == userId);

            if (trainingPlanOffer == null) { return Task.FromResult(false).Result; }

            trainingPlanOffer.Status = newStatus;

            _dbContext.TrainingPlanOffers.Update(trainingPlanOffer);
            await _dbContext.SaveChangesAsync();

            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }

    public async Task<bool> DeleteTrainingPlanOffer(int trainerId, int trainingPlanId)
    {
        try
        {
            var trainingPlanOffer = await _dbContext.TrainingPlanOffers.FirstOrDefaultAsync(x => x.Id == trainingPlanId && x.Trainer.Id == trainerId);

            if (trainingPlanOffer == null) { return Task.FromResult(false).Result; }

            _dbContext.TrainingPlanOffers.Remove(trainingPlanOffer);
            await _dbContext.SaveChangesAsync();

            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }

    public async Task<bool> PostTrainingPlanOffer(int trainerId, TrainingPlanOfferPostDto dto)
    {
        var trainer = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == trainerId);

        if (trainer == null)
        {
            return Task.FromResult(false).Result;
        }

        try
        {
            var existingOffer = await _dbContext.TrainingPlanOffers.FirstOrDefaultAsync(x => x.Trainer.Id == trainerId && x.TrainingPlanForm.Id == dto.TrainingPlanFormId);

            if (existingOffer != null)
            {
                existingOffer.Status = DataAccess.Enumerators.OfferStatus.Offered;
                existingOffer.PriceFrom = dto.PriceFrom;
                existingOffer.PriceTo = dto.PriceTo;
                existingOffer.Details = dto.Details;
                existingOffer.OfferedDate = DateTime.Now;

                _dbContext.TrainingPlanOffers.Update(existingOffer);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var trainingPlanForm = await _dbContext.TrainingPlanForms.FirstOrDefaultAsync(x => x.Id == dto.TrainingPlanFormId);

                if (trainingPlanForm == null)
                {
                    return Task.FromResult(false).Result;
                }

                var trainingPlanOffer = new TrainingPlanOffer
                {
                    Trainer = trainer,
                    Details = dto.Details,
                    PriceFrom = dto.PriceFrom,
                    PriceTo = dto.PriceTo,
                    OfferedDate = DateTime.Now,
                    Status = DataAccess.Enumerators.OfferStatus.Offered,
                    TrainingPlanForm = trainingPlanForm,
                };

                await _dbContext.TrainingPlanOffers.AddAsync(trainingPlanOffer);
                await _dbContext.SaveChangesAsync();
            }

            return Task.FromResult(true).Result;
        }
        catch
        {
            return Task.FromResult(false).Result;
        }
    }
}
