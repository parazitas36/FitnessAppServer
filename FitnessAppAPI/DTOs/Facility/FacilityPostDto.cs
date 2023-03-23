using FitnessAppAPI.DTOs.User;

namespace FitnessAppAPI.DTOs.Facility;

public class FacilityPostDto
{
    public ContactInfoDto ContactInfo { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Coordinates { get; set; }
}
