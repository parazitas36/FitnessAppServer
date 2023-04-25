namespace FitnessAppAPI.DTOs.Forms;

public class TrainerJobFormGetDto
{
    public int Id { get; set; }
    public int TrainerId { get; set; }
    public string TrainerUsername { get; set; }
    public string? Education { get; set; }
    public string? PersonalAchievements { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string? OtherDetails { get; set; }
    public DateTime CreationDate { get; set; }
}
