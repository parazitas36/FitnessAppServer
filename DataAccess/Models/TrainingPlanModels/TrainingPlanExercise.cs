using DataAccess.Enumerators;
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

    public Days? Day { get; set; }

    public int? Week { get; set; }

    public DateTime? ScheduledStartDate { get; set; }

    public DateTime? ScheduledEndDate { get; set; }

    [Required]
    [StringLength(50)]
    public string Sets { get; set; }
}
