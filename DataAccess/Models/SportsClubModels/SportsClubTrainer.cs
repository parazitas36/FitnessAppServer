using DataAccess.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.SportsClubModels;

[Table("SportsClubTrainer")]
public class SportsClubTrainer : Entity
{
    [Required]
    public User Trainer { get; set; }

    [Required]
    public SportsClub SportsClub { get; set; }
}
