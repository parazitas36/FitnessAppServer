namespace FitnessAppAPI.DTOs.TrainingPlan;

public class ScheduledPlanExercisePostDto
{
    public int Id { get; set; }
    public string Sets { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
}
