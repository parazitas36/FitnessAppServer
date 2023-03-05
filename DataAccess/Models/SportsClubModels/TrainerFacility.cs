using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.SportsClubModels;

[Table("TrainerFacility")]
public class TrainerFacility : Entity
{
    [Required]
    public SportsClubTrainer Trainer { get; set; }

    [Required]
    public Facility Facility { get; set;}
}
