namespace FitnessAppAPI.DTOs.TrainingPlanProgress;

public class DayProgressGetDto
{
    public string Day { get; set; }
    public string Sets { get; set; }
    public string? LoggedSets { get; set;}
}
