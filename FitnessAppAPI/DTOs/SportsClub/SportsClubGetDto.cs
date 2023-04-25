using FitnessAppAPI.DTOs.User;

namespace FitnessAppAPI.DTOs.SportsClub;

public class SportsClubGetDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? TrainersCount { get; set; }
    public int? FacilitiesCount { get; set; }
    public int OwnerId { get; set; }
    public string LogoUri { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}
