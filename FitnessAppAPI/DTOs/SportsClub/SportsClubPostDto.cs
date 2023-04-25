using FitnessAppAPI.DTOs.User;

namespace FitnessAppAPI.DTOs.SportsClub;

public class SportsClubPostDto
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }
    
    public IFormFile Logo { get; set; }
}
