using FitnessAppAPI.DTOs.User;

namespace FitnessAppAPI.DTOs.SportsClub;

public class SportsClubGetDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int OwnerId { get; set; }
}
