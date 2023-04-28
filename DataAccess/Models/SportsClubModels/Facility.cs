using DataAccess.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.SportsClubModels;

[Table("Facility")]
public class Facility : Entity
{
    [Required]
    public ContactInfo ContactInfo { get; set; }

    [Required]
    public SportsClub SportsClub { get; set; }

    [Required]
    [StringLength(50)]
    public string Country { get; set; }

    [Required]
    [StringLength(100)]
    public string City { get; set; }

    [Required]
    [StringLength(100)]
    public string ImageUri { get; set; }
}
