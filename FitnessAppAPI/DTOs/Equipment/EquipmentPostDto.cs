﻿namespace FitnessAppAPI.DTOs.Equipment;

public class EquipmentPostDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IFormFile Image { get; set; }
}
