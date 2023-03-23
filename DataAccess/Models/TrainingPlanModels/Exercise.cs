using DataAccess.Enumerators;
using DataAccess.Models.SportsClubModels;
using DataAccess.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.TrainingPlanModels;

[Table("Exercise")]
public class Exercise : Entity
{
    [Required]
    public User CreatedBy { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(50)]
    public string MuscleGroups { get; set; }

    [Required]
    public ExerciseTypes ExerciseType { get; set; }

    public Equipment? Equipment { get; set; }
}
