using DataAccess.DatabaseContext;
using DataAccess.Models.SportsClubModels;
using DataAccess.Models.UserModels;
using FitnessAppAPI.DTOs.Equipment;
using FitnessAppAPI.DTOs.Facility;
using FitnessAppAPI.DTOs.User;
using FitnessAppAPI.Services.Helpers;
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
            Country = y.Country,
            SportsClubId = sportsClubId,
            ContactInfo = new ContactInfoDto
            {
                Email = y.ContactInfo.EmailAddress,
                PhoneNumber = y.ContactInfo.PhoneNumber,
            },
            ImageUri = y.ImageUri,
        }).ToList());
    }

    public Task<List<EquipmentGetDto>> GetFacilityEquipment(int facilityId)
    {
        return Task.FromResult(_dbContext.FacilitiesEquipment.Include(x => x.Equipment).Where(x => x.Facility.Id == facilityId).Select(x => new EquipmentGetDto
        {
            Id = x.Equipment.Id,
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
                EmailAddress = facility.Email,
                PhoneNumber = facility.PhoneNumber
            })).Entity;

            var imageUri = await FilesHandler.SaveFile(facility.Image);

            await _dbContext.Facilities.AddAsync(new Facility
            {
                City = facility.City,
                ContactInfo = contactInfo,
                Country = facility.Country,
                SportsClub = sportsClub,
                ImageUri = imageUri,
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

        if (equipment == null || facility == null || amount < 0)
        {
            return false;
        }

        var facilityEquipment = await _dbContext.FacilitiesEquipment.FirstOrDefaultAsync(x => x.Equipment.Id == equipmentId && x.Facility.Id == facilityId);

        try
        {
            if (facilityEquipment == null && amount > 0)
            {
                await _dbContext.FacilitiesEquipment.AddAsync(new FacilityEquipment
                {
                    Facility = facility,
                    Equipment = equipment,
                    Amount = amount
                });
            }
            else if(facilityEquipment != null)
            {
                if (amount > 0)
                {
                    facilityEquipment.Amount = amount;

                    _dbContext.FacilitiesEquipment.Update(facilityEquipment);
                }
                else if (amount == 0)
                {
                    _dbContext.FacilitiesEquipment.Remove(facilityEquipment);
                }
                else { return false; }
            }
            else
            {
                return false;
            }

            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }
}
