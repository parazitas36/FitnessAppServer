namespace FitnessAppAPI.DTOs.Forms;

public class TrainerInviteGetDto
{
    public int Id { get; set; }
    public string Status { get; set; }
    public int TrainerId { get; set; }
    public int SportsClubId { get; set; }
    public string SportsClub { get; set; }
    public DateTime Date { get; set; }
}
