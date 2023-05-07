namespace FitnessAppAPI.DTOs.Forms;

public class BodyMeasurementsPostDto
{
    public double Height { get; set; }

    public double Weight { get; set; }

    public double? Shoulders { get; set; }

    public double? Chest { get; set; }

    public double? Waist { get; set; }

    public double? Hips { get; set; }
    public IFormFile? Image { get; set; }
}
