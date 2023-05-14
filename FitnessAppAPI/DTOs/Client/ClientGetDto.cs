namespace FitnessAppAPI.DTOs.Client;

public class ClientGetDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public int TrainingPlansAssigned { get; set; }
}
