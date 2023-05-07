namespace FitnessAppAPI.DTOs.Forms;

public class BodyMeasurementsGetDto
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public double Height { get; set; }

    public double Weight { get; set; }

    public double? Shoulders { get; set; }

    public double? Chest { get; set; }

    public double? Waist { get; set; }

    public double? Hips { get; set; }

    public DateTime? MeasurementDay { get; set; }
    
    public string? ImageUri { get; set; }
}
