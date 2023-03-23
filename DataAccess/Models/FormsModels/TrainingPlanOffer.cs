using DataAccess.Enumerators;
using DataAccess.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.FormsModels;

[Table("TrainingPlanOffer")]
public class TrainingPlanOffer : Entity
{
    [Required]
    public TrainingPlanForm TrainingPlanForm { get; set; }

    [Required]
    public User Trainer { get; set; }

    [StringLength(250)]
    public string? Details { get; set; }

    [Required]
    public double PriceFrom { get; set; }

    [Required]
    public double PriceTo { get; set; }

    [Required]
    public OfferStatus Status { get; set; }

    [Required]
    public DateTime OfferedDate { get; set; }
}
