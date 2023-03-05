using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.TrainingPlanModels;

[Table("Exercise")]
public class ExerciseGuide : Entity
{
    [Required]
    public Exercise Exercise { get; set; }

    [Required]
    public string Guide { get; set; }
}
