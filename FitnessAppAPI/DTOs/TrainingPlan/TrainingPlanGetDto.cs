namespace FitnessAppAPI.DTOs.TrainingPlan;

public class TrainingPlanGetDto
{
    public int TrainingPlanId { get; set; }
    public string Name { get; set; }
    public List<WeeklyPlanWeekGetDto>? WeeklyPlan { get; set; }
}
