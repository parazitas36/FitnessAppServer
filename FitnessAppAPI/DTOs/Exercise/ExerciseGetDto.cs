﻿namespace FitnessAppAPI.DTOs.Exercise;

using DataAccess.Models.SportsClubModels;

public class ExerciseGetDto
{
    public int Id { get; set; }
    public int CreatedBy { get; set; }
    public string Name { get; set; }
    public string MuscleGroups { get; set; }
    public string ExerciseType { get; set; }
    public Equipment? Equipment { get; set; }
    public bool HasGuide { get; set; }
}
