using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.TrainingPlanModels;

[Table("ExerciseProgress")]
public class ExerciseProgress : Entity
{
    [Required]
    public TrainingPlanExercise TrainingPlanExercise { get; set; }

    public DateTime? CompletionDate { get; set; }

    [Required]
    public string LoggedSets { get; set; }
}
