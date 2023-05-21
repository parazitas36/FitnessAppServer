using DataAccess.DatabaseContext;
using DataAccess.Enumerators;
using DataAccess.Models.SportsClubModels;
using FitnessAppAPI.DTOs.Equipment;
using FitnessAppAPI.DTOs.SportsClub;
using FitnessAppAPI.DTOs.User;
using FitnessAppAPI.Services.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessAppAPI.Services.Logic;

public class SportsClubLogic : ISportsClubLogic
{
    private readonly FitnessAppDbContext _dbContext;

    public SportsClubLogic(FitnessAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SportsClubGetDto> CreateSportsClub(int ownerId, SportsClubPostDto sportsClub)
    {
        var existingClub = await _dbContext.SportsClubs.FirstOrDefaultAsync(x => x.Owner.Id == ownerId || x.Name == sportsClub.Name);
        var owner = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == ownerId);

        if (existingClub != null || owner == null)
        {
            return null;
        }

        try
        {
            if (sportsClub.PhoneNumber != null || sportsClub.Email != null)
            {
                var contactInfo = await _dbContext.ContactInfo.AddAsync(new DataAccess.Models.UserModels.ContactInfo
                {
                    EmailAddress = sportsClub.Email,
                    PhoneNumber = sportsClub.PhoneNumber,
                });

                owner.ContactInfo = contactInfo.Entity;
                _dbContext.Users.Update(owner);
            }

            string logoUri = await FilesHandler.SaveFile(sportsClub.Logo);

            var newClub = (await _dbContext.SportsClubs.AddAsync(new SportsClub
            {
                Owner = owner,
                Name = sportsClub.Name,
                Description = sportsClub.Description,
                LogoUri = logoUri,
            })).Entity;

            await _dbContext.SaveChangesAsync();

            return new SportsClubGetDto
            {
                OwnerId = ownerId,
                Name = sportsClub.Name,
                Description = sportsClub.Description,
                Id = newClub.Id,
                LogoUri = logoUri,
                FacilitiesCount = 0,
                TrainersCount = 0,
                Email = sportsClub.Email,
                PhoneNumber= sportsClub.PhoneNumber,
            };
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<SportsClubGetDto>> GetAllSportsClubs(int page)
    {
        return Task.FromResult(await _dbContext.SportsClubs.Skip((page - 1) * 50).Include(x => x.Owner).Include(x => x.Owner.ContactInfo)
            .Select(x => new SportsClubGetDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                OwnerId = x.Owner.Id,
                LogoUri = x.LogoUri,
                TrainersCount = _dbContext.SportsClubTrainers.Where(y => y.SportsClub.Id == x.Id).Count(),
                FacilitiesCount = _dbContext.Facilities.Where(y => y.SportsClub.Id == x.Id).Count(),
                PhoneNumber = x.Owner.ContactInfo.PhoneNumber,
                Email = x.Owner.Email
            }).ToListAsync()).Result;
    }

    public async Task<SportsClubGetDto?> GetSportsClubById(int id, bool full = false)
    {
        var sportsClub = await _dbContext.SportsClubs.Include(x => x.Owner).Include(x => x.Owner.ContactInfo).Where(x => x.Id == id)
            .Select(x => new SportsClubGetDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                OwnerId = x.Owner.Id,
                LogoUri = x.LogoUri,
                TrainersCount = _dbContext.SportsClubTrainers.Where(y => y.SportsClub.Id == x.Id).Count(),
                FacilitiesCount = _dbContext.Facilities.Where(y => y.SportsClub.Id == x.Id).Count(),
                PhoneNumber = x.Owner.ContactInfo.PhoneNumber,
                AverageRating = _dbContext.Reviews.Where(y => y.SportsClub.Id == x.Id).Average(y => y.Rating),
                ReviewsCount = _dbContext.Reviews.Count(y => y.SportsClub.Id == x.Id),
                Email = x.Owner.Email,
                Reviews = _dbContext.Reviews.Include(y => y.CreatedBy).Where(y => y.SportsClub.Id == id)
                .Select(r => new ReviewGetDto
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Review = r.ReviewText,
                    User = new UserShortGetDto
                    {
                        Id = r.CreatedBy.Id,
                        Username = r.CreatedBy.Username
                    }
                }).ToList()
            }).FirstOrDefaultAsync();


        return Task.FromResult(sportsClub).Result;
    }

    public async Task<bool> CreateSubscription(SubscriptionPostDto dto, int sportsClubId, int userId)
    {
        var sportsClub = await _dbContext.SportsClubs.FirstOrDefaultAsync(x => x.Id == sportsClubId && x.Owner.Id == userId);

        if (sportsClub == null)
        {
            return false;
        }

        try
        {
            await _dbContext.Subscriptions.AddAsync(new Subscription
            {
                SportsClub = sportsClub,
                Name = dto.Name,
                Details = dto.Details,
                Price = dto.Price,
            });

            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public Task<List<SubscriptionGetDto>> GetAllSubscriptions(int sportsClubId)
    {
        var result = _dbContext.Subscriptions.Where(x => x.SportsClub.Id == sportsClubId).Select(x => new SubscriptionGetDto
        {
            Id = x.Id,
            Name = x.Name,
            Details = x.Details,
            Price = x.Price,
            SportsClubId = sportsClubId,
        }).ToList();

        return Task.FromResult(result);
    }

    public async Task<SportsClubGetDto?> GetUserSportsClub(int userId)
    {
        var sportsClub = await _dbContext.SportsClubs.Include(x => x.Owner).Where(x => x.Owner.Id == userId).Include(x => x.Owner.ContactInfo)
            .Select(x => new SportsClubGetDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                OwnerId = x.Owner.Id,
                LogoUri = x.LogoUri,
                TrainersCount = _dbContext.SportsClubTrainers.Where(y => y.SportsClub.Id == x.Id).Count(),
                FacilitiesCount = _dbContext.Facilities.Where(y => y.SportsClub.Id == x.Id).Count(),
                PhoneNumber = x.Owner.ContactInfo.PhoneNumber,
                Email = x.Owner.Email
            }).FirstOrDefaultAsync();

        return Task.FromResult(sportsClub).Result;
    }

    public async Task<List<Equipment>> GetSportsClubEquipment(int sportsClubId)
    {
        return Task.FromResult(_dbContext.Equipment.Where(x => x.SportsClub.Id == sportsClubId).ToList()).Result;
    }

    public async Task<Equipment> CreateEquipment(int sportsClubId, EquipmentPostDto equipment)
    {
        var sportsClub = await _dbContext.SportsClubs.FirstOrDefaultAsync(x => x.Id == sportsClubId);

        if (sportsClub == null) { return null; }

        var imageUri = await FilesHandler.SaveFile(equipment.Image);

        try
        {
            var newEquipment = (await _dbContext.Equipment.AddAsync(new Equipment
            {
                Name = equipment.Name,
                Description = equipment.Description,
                ImageURI = imageUri,
                SportsClub = sportsClub,
            })).Entity;

            await _dbContext.SaveChangesAsync();

            return newEquipment;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<TrainerGetDto>> GetSportsClubTrainers(int sportsClubId)
    {
        var trainers = await _dbContext.SportsClubTrainers.Where(x => x.SportsClub.Id == sportsClubId).Select(x => x.Trainer)
            .Select(x => new TrainerGetDto
            {
                Id = x.Id,
                Name = x.Name,
                LastName = x.Surname,
                Username = x.Username,
                Email = _dbContext.ContactInfo.FirstOrDefault(y => x.ContactInfo != null && y.Id == x.ContactInfo.Id).EmailAddress,
                Phone = _dbContext.ContactInfo.FirstOrDefault(y => x.ContactInfo != null && y.Id == x.ContactInfo.Id).PhoneNumber,
                AverageRating = _dbContext.Reviews.Where(y => y.Trainer.Id == x.Id).Average(y => y.Rating),
                ReviewsCount = _dbContext.Reviews.Count(y => y.Trainer.Id == x.Id),
            }).ToListAsync();

        return Task.FromResult(trainers).Result;
    }
}
