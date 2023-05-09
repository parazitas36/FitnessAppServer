namespace FitnessAppAPI.DTOs.TrainingPlan;

public class UserWeeklyPlanWeekGetDto
{
    public int Week { get; set; }
    public UserWeeklyPlanDaysGetDto Days { get; set; }
}
