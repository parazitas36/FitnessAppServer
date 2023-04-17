namespace FitnessAppAPI.DTOs.TrainingPlan;

public class WeeklyPlanWeekPostDto
{
    public int Week { get; set; }
    public WeeklyPlanDaysPostDto Days { get; set; }
}
