namespace FitnessAppAPI.DTOs.TrainingPlan;

public class WeeklyPlanWeekGetDto
{
    public int Week { get; set; }
    public WeeklyPlanDaysGetDto Days { get; set; }
}
