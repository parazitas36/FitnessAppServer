using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.UserModels;

[Table("BodyMeasurements")]
public class BodyMeasurements : Entity
{
    [Required]
    public User User { get; set; }

    public double Height { get; set; }

    public double Weight { get; set; }

    public double? Shoulders { get; set; }

    public double? Chest { get; set; }

    public double? Waist { get; set; }

    public double? Hips { get; set; }

    [Required]
    public DateTime MeasurementDay { get; set; } = DateTime.Now;
    
    [Required]
    public bool ImperialSystem { get; set; }

    public string? ImageUri { get; set; }
}
