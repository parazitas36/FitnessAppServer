using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.SportsClubModels;

[Table("Equipment")]
public class Equipment : Entity
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(200)]
    public string Description { get; set; }

    [Required]
    [StringLength(100)]
    public string ImageURI { get; set; }
}
