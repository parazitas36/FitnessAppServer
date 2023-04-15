using DataAccess.Models.UserModels;
using FitnessAppAPI.DTOs.User;

namespace FitnessAppAPI.Services.Interfaces;

public interface IUsersLogic
{
    Task<UserGetDto> Login(string username, string password);

    Task<bool> Register(UserRegisterDto user);
}
