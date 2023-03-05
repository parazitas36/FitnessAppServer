using DataAccess.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.FormsModels;

[Table("TrainingPlanForm")]
public class TrainingPlanForm : Entity
{
    [Required]
    public User CreatedBy { get; set; }

    [Required]
    public string FormDetails { get; set; }
}
