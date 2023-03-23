using FitnessAppAPI.DTOs.User;

namespace FitnessAppAPI.DTOs.SportsClub;

public class SportsClubPostDto
{
    public string Name { get; set; }

    public string Description { get; set; }

    public ContactInfoDto ContactInfo { get; set; }
}
