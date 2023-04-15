namespace FitnessAppAPI.DTOs.Equipment;

public class EquipmentGetDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageURI { get; set; }
    public int Amount { get; set; }
    public int FacilityId { get; set; }
}
