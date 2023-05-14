namespace FitnessAppAPI.DTOs.TrainingPlanProgress;

public class WeekProgressGetDto
{
    public int Week { get; set; }
    public List<DayProgressGetDto> Days { get; set; }
}
