using DataAccess.DatabaseContext;
using DataAccess.Models.SportsClubModels;
using FitnessAppAPI.DTOs.Equipment;
using FitnessAppAPI.DTOs.SportsClub;
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
            var newClub = (await _dbContext.SportsClubs.AddAsync(new SportsClub
            {
                Owner = owner,
                Name = sportsClub.Name,
                Description = sportsClub.Description
            })).Entity;

            await _dbContext.SaveChangesAsync();
            return new SportsClubGetDto
            {
                OwnerId = ownerId,
                Name = sportsClub.Name,
                Description = sportsClub.Description,
                Id = newClub.Id
            };
        }
        catch
        {
            return null;
        }
    }

    public Task<List<SportsClubGetDto>> GetAllSportsClubs(int page)
    {
        return Task.FromResult(_dbContext.SportsClubs.Skip((page - 1) * 50).Select(x => new SportsClubGetDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            OwnerId = x.Owner.Id
        }).ToList());
    }

    public async Task<SportsClubGetDto?> GetSportsClubById(int id)
    {
        var sportsClub = await _dbContext.SportsClubs.Include(x => x.Owner).FirstOrDefaultAsync(x => x.Id == id);

        return sportsClub is null ? null : new SportsClubGetDto
        {
            Id = id,
            Description = sportsClub.Description,
            Name = sportsClub.Name,
            OwnerId = sportsClub.Owner.Id
        };
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
            SportsClubId = sportsClubId
        }).ToList();

        return Task.FromResult(result);
    }

    public async Task<SportsClubGetDto?> GetUserSportsClub(int userId)
    {
        var result = await _dbContext.SportsClubs.FirstOrDefaultAsync(x => x.Owner.Id == userId);

        if (result == null)
        {
            return default;
        }

        return new SportsClubGetDto
        {
            Id = result.Id,
            Description = result?.Description,
            Name = result.Name,
            OwnerId = userId
        };
    }

    public async Task<List<Equipment>> GetSportsClubEquipment(int sportsClubId)
    {
        return Task.FromResult(_dbContext.Equipment.Where(x => x.SportsClub.Id == sportsClubId).ToList()).Result;
    }

    public async Task<Equipment> CreateEquipment(int sportsClubId, EquipmentPostDto equipment)
    {
        var sportsClub = await _dbContext.SportsClubs.FirstOrDefaultAsync(x => x.Id == sportsClubId);

        if (sportsClub == null) { return null; }

        try
        {
            var newEquipment = (await _dbContext.Equipment.AddAsync(new Equipment
            {
                Name = equipment.Name,
                Description = equipment.Description,
                ImageURI = equipment.ImageUri,
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
}
