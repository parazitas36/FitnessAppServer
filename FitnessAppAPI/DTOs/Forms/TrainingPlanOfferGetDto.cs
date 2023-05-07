using FitnessAppAPI.DTOs.User;

namespace FitnessAppAPI.DTOs.Forms;

public class TrainingPlanOfferGetDto
{
    public int TrainingPlanOfferId { get; set; }
    public TrainerGetDto? Trainer { get; set; }
    public UserGetDto? CreatedBy { get; set; }
    public string Details { get; set; }
    public double PriceFrom { get; set; }
    public double PriceTo { get; set; }
    public string Status { get; set; }
}
