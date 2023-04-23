namespace FitnessAppAPI.DTOs.TrainingPlan;

public class WeeklyPlanDaysGetDto
{
    public List<ExercisesWithSetsGetDto>? Monday { get; set; }
    public List<ExercisesWithSetsGetDto>? Tuesday { get; set; }
    public List<ExercisesWithSetsGetDto>? Wednesday { get; set; }
    public List<ExercisesWithSetsGetDto>? Thursday { get; set; }
    public List<ExercisesWithSetsGetDto>? Friday { get; set; }
    public List<ExercisesWithSetsGetDto>? Saturday { get; set; }
    public List<ExercisesWithSetsGetDto>? Sunday { get; set; }
}
