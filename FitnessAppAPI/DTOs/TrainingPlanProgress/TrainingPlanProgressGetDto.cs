namespace FitnessAppAPI.DTOs.TrainingPlanProgress;

public class TrainingPlanProgressGetDto
{
    public int ExerciseId { get; set; }
    public string ExerciseName { get; set; }
    public List<WeekProgressGetDto> Weeks { get; set; }
}
