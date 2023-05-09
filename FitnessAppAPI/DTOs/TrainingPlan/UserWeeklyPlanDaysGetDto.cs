namespace FitnessAppAPI.DTOs.TrainingPlan;

public class UserWeeklyPlanDaysGetDto
{
    public List<UserExercisesWithSetsGetDto>? Monday { get; set; }
    public List<UserExercisesWithSetsGetDto>? Tuesday { get; set; }
    public List<UserExercisesWithSetsGetDto>? Wednesday { get; set; }
    public List<UserExercisesWithSetsGetDto>? Thursday { get; set; }
    public List<UserExercisesWithSetsGetDto>? Friday { get; set; }
    public List<UserExercisesWithSetsGetDto>? Saturday { get; set; }
    public List<UserExercisesWithSetsGetDto>? Sunday { get; set; }
}
