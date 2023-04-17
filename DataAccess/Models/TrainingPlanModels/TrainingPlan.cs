using DataAccess.Enumerators;
using DataAccess.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.TrainingPlanModels;

[Table("TrainingPlan")]
public class TrainingPlan : Entity
{
    [Required]
    public User CreatedBy { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    public TrainingPlanType TrainingPlanType { get; set; }
}
