namespace FitnessAppAPI.DTOs.SportsClub;

public class SubscriptionGetDto
{
    public int Id { get; set; }
    public int SportsClubId { get; set; }
    public string Name { get; set; }
    public string Details { get; set; }
    public double Price { get; set; }
}
