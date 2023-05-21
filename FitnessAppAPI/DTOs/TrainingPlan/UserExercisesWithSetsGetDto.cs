using FitnessAppAPI.DTOs.Equipment;

namespace FitnessAppAPI.DTOs.TrainingPlan;

public class UserExercisesWithSetsGetDto
{
    public int TrainingPlanExerciseId { get; set; }
    public int ExerciseId { get; set; }
    public bool? HasGuide { get; set; }
    public string ExerciseName { get; set; }
    public string MuscleGroups { get; set; }
    public EquipmentGetDto? Equipment { get; set; }
    public string Sets { get; set; }
    public string? LoggedSets { get; set; }
}
