namespace FitnessAppAPI.DTOs.User;

public class UserGetDto
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Role { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }
    
    public bool IsPublicName { get; set; }

    public bool UsesImperialSystem { get; set; }

    public ContactInfoDto? ContactInfo { get; set; }
}
