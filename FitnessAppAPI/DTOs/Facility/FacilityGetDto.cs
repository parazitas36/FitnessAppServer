using FitnessAppAPI.DTOs.User;

namespace FitnessAppAPI.DTOs.Facility;

public class FacilityGetDto
{
    public int Id { get; set; }
    public ContactInfoDto ContactInfo { get; set; }
    public int SportsClubId { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Coordinates { get; set; }
}
