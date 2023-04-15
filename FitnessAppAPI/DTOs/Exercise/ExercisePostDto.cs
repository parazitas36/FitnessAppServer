namespace FitnessAppAPI.DTOs.Exercise;

public class ExercisePostDto
{
    public string Name { get; set; }
    public string MuscleGroups { get; set; }
    public string ExerciseTypes { get; set; }
    public int? EquipmentId { get; set; }
    public string? Guide { get; set; }
}
