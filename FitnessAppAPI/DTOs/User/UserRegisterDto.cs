using DataAccess.Enumerators;
using DataAccess.Models.UserModels;

namespace FitnessAppAPI.DTOs.User;

public class UserRegisterDto
{
    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string RepeatPassword { get; set; }

    public int Role { get; set; }

    public ContactInfoDto? ContactInfo { get; set; }

    public bool UsesImperialSystem { get; set; } = false;

    public string Name { get; set; }

    public string Surname { get; set; }

    public bool IsPublicName { get; set; } = false;
}
