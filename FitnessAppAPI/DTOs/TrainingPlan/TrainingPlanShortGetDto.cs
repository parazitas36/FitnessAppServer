namespace FitnessAppAPI.DTOs.TrainingPlan;

using DataAccess.Models.SportsClubModels;

public class TrainingPlanShortGetDto
{
    public int Id { get; set; }
    public int CreatedById { get; set; }
    public string Name { get; set; }
    public List<string> MuscleGroups { get; set; }
    public List<Equipment>? Equipment { get; set; }
}
