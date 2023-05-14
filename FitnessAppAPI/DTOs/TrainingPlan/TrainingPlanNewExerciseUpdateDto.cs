namespace FitnessAppAPI.DTOs.TrainingPlan;

public class TrainingPlanNewExerciseUpdateDto
{
    public int Week { get; set; }
    public string Day { get; set; }
    public int ExerciseId { get; set; }
    public string Sets { get; set; }
}
