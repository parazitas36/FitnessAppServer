namespace FitnessAppAPI.DTOs.Forms;

public class BodyMeasurementsGetDto
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public double? Weight { get; set; }

    public double? Bust { get; set; }

    public double? Waist { get; set; }

    public double? Hip { get; set; }

    public DateTime? MeasurementDay { get; set; }
    
    public string ImageUri { get; set; }
}
