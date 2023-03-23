using DataAccess.Enumerators;
using DataAccess.Models.SportsClubModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.FormsModels;

[Table("JobOffer")]
public class JobOffer : Entity
{
    [Required]
    public TrainerJobForm TrainerJobForm { get; set; }

    [Required]
    public SportsClub SportsClub { get; set; }

    [Required]
    [StringLength(250)]
    public string Details { get; set; }

    [Required]
    public OfferStatus Status { get; set; }

    [Required]
    public DateTime OfferDate { get; set; } = DateTime.Now;
}
