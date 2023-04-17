namespace FitnessAppAPI.DTOs.Exercise;

public class ExerciseWithGuidePostDto
{
    public List<string>? BlockTypes { get; set; }
    public List<string>? Texts { get; set; }
    public List<IFormFile>? Files { get; set;}
    public string ExerciseTypes { get; set; }
    public string MuscleGroups { get; set; }
    public string Name { get; set; }
    public int? EquipmentId { get; set; }
}
