using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.TrainingPlanModels;

[Table("TrainingPlan_Exercise")]
public class TrainingPlanExercise : Entity
{
    [Required]
    public TrainingPlan TrainingPlan { get; set; }

    [Required]
    public Exercise Exercise { get; set; }

    [Required]
    public DateTime ScheduledStartDate { get; set; }

    [Required]
    public DateTime ScheduledEndDate { get; set; }

    [Required]
    [StringLength(50)]
    public string Sets { get; set; }
}
