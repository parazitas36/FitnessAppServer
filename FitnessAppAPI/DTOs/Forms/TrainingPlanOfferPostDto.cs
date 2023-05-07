namespace FitnessAppAPI.DTOs.Forms;

public class TrainingPlanOfferPostDto
{
    public int TrainingPlanFormId { get; set; }
    public string Details { get; set; }
    public double PriceFrom { get; set; }
    public double PriceTo { get; set;}
}
