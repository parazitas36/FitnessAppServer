using FitnessAppAPI.DTOs.User;

namespace FitnessAppAPI.DTOs.Facility;

public class FacilityPostDto
{
    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public IFormFile Image { get; set; }
}
