using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.TrainingPlanModels;

[Table("ExerciseProgress")]
public class ExerciseProgress : Entity
{
    [Required]
    TrainingPlanExercise TrainingPlanExercise { get; set; }

    public DateTime? CompletionDate { get; set; }

    [Required]
    [StringLength(100)]
    public string LoggedSets { get; set; }
}
