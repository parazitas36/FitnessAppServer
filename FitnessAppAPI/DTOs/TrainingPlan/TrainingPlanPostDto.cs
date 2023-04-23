namespace FitnessAppAPI.DTOs.TrainingPlan;

public class TrainingPlanPostDto
{
    public string Name { get; set; }
    public List<WeeklyPlanWeekPostDto>? WeeklyPlan { get; set; }
}
