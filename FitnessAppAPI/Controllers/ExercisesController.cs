using FitnessAppAPI.DTOs.Exercise;
using FitnessAppAPI.Services.Helpers;
using FitnessAppAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        return Ok(await _exercisesLogic.GetTrainersExerices(userId));
    }

    [HttpPost]
    [Authorize(Roles = "Trainer")]
    public async Task<ActionResult> CreateExercise([FromBody] ExercisePostDto dto)
    {
        var userId = JwtHelper.GetUserId(Request);

        bool isCreated = await _exercisesLogic.CreateExercise(userId, dto);

        return isCreated ? Created(nameof(ExerciseGetDto), null) : BadRequest();
    }
}
