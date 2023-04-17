namespace FitnessAppAPI.DTOs.TrainingPlan;

public class WeeklyPlanDaysPostDto
{
    public List<ExercisesWithSetsPostDto>? Monday { get; set; }
    public List<ExercisesWithSetsPostDto>? Tuesday { get; set; }
    public List<ExercisesWithSetsPostDto>? Wednesday { get; set; }
    public List<ExercisesWithSetsPostDto>? Thursday { get; set; }
    public List<ExercisesWithSetsPostDto>? Friday { get; set; }
    public List<ExercisesWithSetsPostDto>? Saturday { get; set; }
    public List<ExercisesWithSetsPostDto>? Sunday { get; set; }
}
