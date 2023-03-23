using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.UserModels;

[Table("BodyMeasurements")]
public class BodyMeasurements : Entity
{
    [Required]
    public User User { get; set; }

    public double? Weight { get; set; }

    public double? Bust { get; set; }

    public double? Waist { get; set; }

    public double? Hip { get; set; }

    [Required]
    public DateTime MeasureMentDay { get; set; } = DateTime.Now;
    
    [Required]
    public bool ImperialSystem { get; set; }

    public string? PictureURI { get; set; }
}
