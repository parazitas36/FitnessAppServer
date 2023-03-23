using DataAccess.Models.SportsClubModels;
using FitnessAppAPI.DTOs.Equipment;
using FitnessAppAPI.DTOs.Facility;

namespace FitnessAppAPI.Services.Interfaces
{
    public interface IFacilityLogic
    {
        Task<bool> AssignEquipmentToFacility(int facilityId, int equipmentId, int amount);
        Task<List<Equipment>> GetFacilityEquipment(int facilityId);
        Task<Equipment> CreateEquipment(EquipmentPostDto equipment);
        Task<bool> CreateFacility(FacilityPostDto facility, int sportsClubId, int ownerId);
        Task<List<FacilityGetDto>> GetSportsClubFacilities(int sportsClubId);
    }
}