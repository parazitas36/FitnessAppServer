using DataAccess.Models.SportsClubModels;
using FitnessAppAPI.DTOs.Equipment;
using FitnessAppAPI.DTOs.SportsClub;
using FitnessAppAPI.DTOs.User;

namespace FitnessAppAPI.Services.Logic
{
    public interface ISportsClubLogic
    {
        Task<SportsClubGetDto> CreateSportsClub(int ownerId, SportsClubPostDto sportsClub);
        Task<bool> CreateSubscription(SubscriptionPostDto dto, int sportsClubId, int userId);
        Task<List<SportsClubGetDto>> GetAllSportsClubs(int page);
        Task<List<SubscriptionGetDto>> GetAllSubscriptions(int sportsClubId);
        Task<SportsClubGetDto?> GetSportsClubById(int id, bool full = false);
        Task<SportsClubGetDto?> GetUserSportsClub(int userId);
        Task<List<Equipment>> GetSportsClubEquipment(int sportsClubId);
        Task<Equipment> CreateEquipment(int sportsClubId, EquipmentPostDto equipment);
        Task<List<TrainerGetDto>> GetSportsClubTrainers(int sportsClubId);
    }
}