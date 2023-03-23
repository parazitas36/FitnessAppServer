using DataAccess.DatabaseContext;
using DataAccess.Enumerators;
using DataAccess.Models.UserModels;
using FitnessAppAPI.DTOs.User;
using FitnessAppAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessAppAPI.Services.Logic;

public class UsersLogic : IUsersLogic
{
    private readonly FitnessAppDbContext _dbContext;

    public UsersLogic(FitnessAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserGetDto> Login(string username, string password)
    {
        User? user = _dbContext.Users.Include(x => x.ContactInfo).FirstOrDefault(x => x.Username == username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return null;
        }

        return new UserGetDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role.ToString(),
            ContactInfo = new ContactInfoDto
            {
                Email = user.ContactInfo?.EmailAddress,
                PhoneNumber = user.ContactInfo?.PhoneNumber,
            },
            Name = user.Name,
            Surname = user.Surname,
            IsPublicName = user.IsPublicName,
            UsesImperialSystem = user.UsesImperialSystem,
        };
    }

    public async Task<bool> Register(UserRegisterDto user)
    {
        if (user.Password != user.RepeatPassword)
        {
            return false;
        }

        User? existingUser = _dbContext.Users.FirstOrDefault(x => x.Username == user.Username || x.Email == user.Email);

        if (existingUser != null)
        {
            return false;
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

        ContactInfo? contactInfo = null;

        try
        {
            if (user.ContactInfo != null)
            {
                contactInfo = (await _dbContext.ContactInfo.AddAsync(new ContactInfo
                {
                    EmailAddress = user.ContactInfo?.Email,
                    PhoneNumber = user.ContactInfo?.PhoneNumber,
                })).Entity;
            }


            await _dbContext.Users.AddAsync(new User
            {
                Username = user.Username,
                Password = passwordHash,
                ContactInfo = contactInfo,
                Email = user.Email,
                IsPublicName = user.IsPublicName,
                Name = user.Name,
                Surname = user.Surname,
                Role = Enum.TryParse<Roles>(user.Role.ToString(), out Roles role) ? role : Roles.User,
                UsesImperialSystem = user.UsesImperialSystem
            });

            await _dbContext.SaveChangesAsync();
        }
        catch
        {
            return false;
        }

        return true;
    }
}
