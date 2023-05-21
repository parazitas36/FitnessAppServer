namespace FitnessAppAPI.DTOs.TrainingPlan;

public class ExercisesWithSetsGetDto
{
    public int? EditKey { get; set; }
    public int TrainingPlanExerciseId { get; set; }
    public int ExerciseId { get; set; }
    public bool? HasGuide { get; set; }
    public string Sets { get; set; }
}
