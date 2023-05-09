namespace FitnessAppAPI.DTOs.TrainingPlan;

public class UserTrainingPlanGetDto
{
    public int TrainingPlanId { get; set; }
    public string Name { get; set; }
    public List<UserWeeklyPlanWeekGetDto>? WeeklyPlan { get; set; }
}
