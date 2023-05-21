using DataAccess.Models.SportsClubModels;
using FitnessAppAPI.DTOs.Equipment;
using FitnessAppAPI.DTOs.Facility;
using FitnessAppAPI.DTOs.User;

namespace FitnessAppAPI.Services.Interfaces
{
    public interface IFacilityLogic
    {
        Task<bool> AssignEquipmentToFacility(int facilityId, int equipmentId, int amount);
        Task<List<EquipmentGetDto>> GetFacilityEquipment(int facilityId);
        Task<bool> CreateFacility(FacilityPostDto facility, int sportsClubId, int ownerId);
        Task<List<FacilityGetDto>> GetSportsClubFacilities(int sportsClubId);
        Task<List<TrainerGetDto>> GetFacilityTrainers(int facilityId);
    }
}