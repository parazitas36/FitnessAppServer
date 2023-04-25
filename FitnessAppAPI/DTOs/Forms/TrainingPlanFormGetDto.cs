namespace FitnessAppAPI.DTOs.Forms;

public class TrainingPlanFormGetDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; }
    public string FormDetails { get; set; }
}
