using DataAccess.DatabaseContext;
using DataAccess.Models.SportsClubModels;
using DataAccess.Models.UserModels;
using FitnessAppAPI.DTOs.Equipment;
using FitnessAppAPI.DTOs.Facility;
using FitnessAppAPI.DTOs.User;
using FitnessAppAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessAppAPI.Services.Logic;

public class FacilityLogic : IFacilityLogic
{
    private readonly FitnessAppDbContext _dbContext;

    public FacilityLogic(FitnessAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<FacilityGetDto>> GetSportsClubFacilities(int sportsClubId)
    {
        return Task.FromResult(_dbContext.Facilities.Where(x => x.SportsClub.Id == sportsClubId).Select(y => new FacilityGetDto
        {
            Id = y.Id,
            City = y.City,
            Coordinates = y.Coordinates,
            Country = y.Country,
            SportsClubId = sportsClubId,
            ContactInfo = new ContactInfoDto
            {
                Email = y.ContactInfo.EmailAddress,
                PhoneNumber = y.ContactInfo.PhoneNumber,
            }
        }).ToList());
    }

    public Task<List<EquipmentGetDto>> GetFacilityEquipment(int facilityId)
    {
        return Task.FromResult(_dbContext.FacilitiesEquipment.Include(x => x.Equipment).Where(x => x.Facility.Id == facilityId).Select(x => new EquipmentGetDto
        {
            Name = x.Equipment.Name,
            Description = x.Equipment.Description,
            ImageURI = x.Equipment.ImageURI,
            Amount = x.Amount,
            FacilityId = facilityId
        }).ToList());
    }

    public async Task<bool> CreateFacility(FacilityPostDto facility, int sportsClubId, int ownerId)
    {
        var sportsClub = await _dbContext.SportsClubs.FirstOrDefaultAsync(x => x.Id == sportsClubId && x.Owner.Id == ownerId);

        if (sportsClub == null)
        {
            return false;
        }

        try
        {
            var contactInfo = (await _dbContext.ContactInfo.AddAsync(new ContactInfo
            {
                EmailAddress = facility.ContactInfo.Email,
                PhoneNumber = facility.ContactInfo.PhoneNumber
            })).Entity;

            await _dbContext.Facilities.AddAsync(new Facility
            {
                City = facility.City,
                ContactInfo = contactInfo,
                Coordinates = facility.Coordinates,
                Country = facility.Country,
                SportsClub = sportsClub,
            });

            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AssignEquipmentToFacility(int facilityId, int equipmentId, int amount)
    {
        var equipment = await _dbContext.Equipment.FirstOrDefaultAsync(x => x.Id == equipmentId);
        var facility = await _dbContext.Facilities.FirstOrDefaultAsync(x => x.Id == facilityId);

        if (equipment == null || facility == null || amount < 1)
        {
            return false;
        }

        try
        {
            await _dbContext.FacilitiesEquipment.AddAsync(new FacilityEquipment
            {
                Facility = facility,
                Equipment = equipment,
                Amount = amount
            });

            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }
}
