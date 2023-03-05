using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.SportsClubModels;

[Table("FacilityEquipment")]
public class FacilityEquipment : Entity
{
    [Required]
    public Facility Facility { get; set; }

    [Required]
    public Equipment Equipment { get; set; }

    [Required]
    public int Amount { get; set; }
}
