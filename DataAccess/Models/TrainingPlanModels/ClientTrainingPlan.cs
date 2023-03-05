using DataAccess.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.TrainingPlanModels;

[Table("Client_TrainingPlan")]
public class ClientTrainingPlan : Entity
{
    [Required]
    public User Client { get; set; }

    [Required]
    public TrainingPlan TrainingPlan { get; set; }
}
