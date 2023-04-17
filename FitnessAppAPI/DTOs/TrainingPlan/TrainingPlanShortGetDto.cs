namespace FitnessAppAPI.DTOs.TrainingPlan;

using FitnessAppAPI.DTOs.Equipment;

public class TrainingPlanShortGetDto
{
    public int CreatedById { get; set; }
    public string Name { get; set; }
    public string MuscleGroups { get; set; }
    public string Type { get; set; }
    public List<EquipmentGetDto>? Equipment { get; set; }
    public int ClientsCount { get; set; }
}
