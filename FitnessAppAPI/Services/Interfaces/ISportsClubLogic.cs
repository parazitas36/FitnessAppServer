using DataAccess.Models.SportsClubModels;
using FitnessAppAPI.DTOs.Equipment;
using FitnessAppAPI.DTOs.SportsClub;

namespace FitnessAppAPI.Services.Logic
{
    public interface ISportsClubLogic
    {
        Task<SportsClubGetDto> CreateSportsClub(int ownerId, SportsClubPostDto sportsClub);
        Task<bool> CreateSubscription(SubscriptionPostDto dto, int sportsClubId, int userId);
        Task<List<SportsClubGetDto>> GetAllSportsClubs(int page);
        Task<List<SubscriptionGetDto>> GetAllSubscriptions(int sportsClubId);
        Task<SportsClubGetDto?> GetSportsClubById(int id);
        Task<SportsClubGetDto?> GetUserSportsClub(int userId);
        Task<List<Equipment>> GetSportsClubEquipment(int sportsClubId);
        Task<Equipment> CreateEquipment(int sportsClubId, EquipmentPostDto equipment);
    }
}