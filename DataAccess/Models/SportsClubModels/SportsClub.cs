using DataAccess.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.SportsClubModels;

[Table("SportsClub")]
public class SportsClub : Entity
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    public User Owner { get; set; }

    public string? Description { get; set; }
}
