namespace FitnessAppAPI.DTOs.TrainingPlan;

public class TrainingPlanPostDto
{
    public string Name { get; set; }
    public string Type { get; set; }
    public List<WeeklyPlanWeekPostDto>? WeeklyPlan { get; set; }
    public List<ScheduledPlanExercisePostDto>? ScheduledPlan { get; set; }
}
