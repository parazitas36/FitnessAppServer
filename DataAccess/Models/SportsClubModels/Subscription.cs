using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.SportsClubModels;

[Table("Subscription")]
public class Subscription : Entity
{
    [Required]
    public SportsClub SportsClub { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string Details { get; set; }

    [Required]
    public double Price { get; set; }
}
