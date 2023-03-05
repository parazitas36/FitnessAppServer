using DataAccess.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.FormsModels;

[Table("TrainerJobForm")]
public class TrainerJobForm : Entity
{
    [Required]
    public User Trainer { get; set; }

    [StringLength(250)]
    public string? Education { get; set; }

    [StringLength(250)]
    public string? PersonalAchievements { get; set; }

    [Required]
    [StringLength(50)]
    public string Location { get; set; }

    [StringLength(250)]
    public string? OtherDetails { get; set; }
}
