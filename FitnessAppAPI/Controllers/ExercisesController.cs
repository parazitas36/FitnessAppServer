using DataAccess.Models.UserModels;
using FitnessAppAPI.DTOs.Exercise;
using FitnessAppAPI.Services.Helpers;
using FitnessAppAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessAppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExercisesController : ControllerBase
{
    private readonly IExercisesLogic _exercisesLogic;

    public ExercisesController(IExercisesLogic exercisesLogic)
    {
        _exercisesLogic = exercisesLogic;
    }

    [HttpGet("all/trainer/{trainerId:int}")]
    [Authorize(Roles = "Trainer")]
    public async Task<ActionResult<List<ExerciseGetDto>>> GetTrainersExercises(int trainerId)
    {
        var userId = JwtHelper.GetUserId(Request);

        if (userId != trainerId) { return Forbid(); }

        var result = await _exercisesLogic.GetTrainersExerices(userId);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpGet("{exerciseId:int}")]
    public async Task<ActionResult<ExerciseWithGuideGetDto>> GetExerciseById(int exerciseId)
    {
        var result = await _exercisesLogic.GetExerciseById(exerciseId);

        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "Trainer")]
    [RequestSizeLimit(100_000_000)]
    public async Task<ActionResult> CreateExercise([FromForm] ExerciseWithGuidePostDto dto)
    {
        var userId = JwtHelper.GetUserId(Request);

        bool isCreated = await _exercisesLogic.CreateExercise(userId, dto);

        return isCreated ? Created(nameof(ExerciseGetDto), null) : BadRequest();
    }
}
