using FitnessAppAPI.DTOs.TrainingPlan;
using FitnessAppAPI.Services.Helpers;
using FitnessAppAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TrainingPlansController : ControllerBase
    {
        private readonly ITrainingPlanLogic _trainingPlanLogic;

        public TrainingPlansController(ITrainingPlanLogic trainingPlanLogic)
        {
            _trainingPlanLogic = trainingPlanLogic;
        }

        [HttpGet("{trainerId:int}/short")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> GetTrainersTrainingPlansShort(int trainerId)
        {
            var userId = JwtHelper.GetUserId(Request);

            if (userId != trainerId) { return Forbid(); }

            var result = await _trainingPlanLogic.GetTrainersTrainingPlanShortList(userId);

            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> CreateTrainingPlan([FromBody] TrainingPlanPostDto dto)
        {
            var userId = JwtHelper.GetUserId(Request);
            var result = await _trainingPlanLogic.CreateTrainingPlan(userId, dto);

            return result ? Created(nameof(TrainingPlanPostDto), null) : BadRequest();
        }

        [HttpGet("{trainingPlanId:int}")]
        public async Task<IActionResult> GetTrainingPlanById(int trainingPlanId)
        {
            var userId = JwtHelper.GetUserId(Request);
            var role = JwtHelper.GetRole(Request);

            if (role == null) { return Forbid(); }

            bool trainer = role == DataAccess.Enumerators.Roles.Trainer;

            if (trainer)
            {
                var trainerResult = await _trainingPlanLogic.GetTrainingPlanById(userId, trainingPlanId);
                return trainerResult == null ? NotFound() : Ok(trainerResult);
            }

            var userResult = await _trainingPlanLogic.GetUserTrainingPlanById(userId, trainingPlanId);

            return userResult is null ? NotFound() : Ok(userResult);
        }

        [HttpPost("assign/{trainingPlanId:int}/{clientId:int}")]
        public async Task<IActionResult> AssignTrainingPlan(int trainingPlanId, int clientId)
        {
            var trainerId = JwtHelper.GetUserId(Request);
            bool result = await _trainingPlanLogic.AssignTrainingPlan(trainerId, clientId, trainingPlanId);

            return result ? Created("", null) : BadRequest();
        }

        [HttpGet("userplans")]
        public async Task<IActionResult> GetUserTrainingPlans()
        {
            var userId = JwtHelper.GetUserId(Request);

            return Ok(await _trainingPlanLogic.GetUsersTrainingPlanShortList(userId));
        }

        [HttpPost("progress/{trainingPlanExerciseId:int}")]
        public async Task<IActionResult> PostLoggedSets([FromBody] string loggedSets, int trainingPlanExerciseId)
        {
            var userId = JwtHelper.GetUserId(Request);

            bool result = await _trainingPlanLogic.LogExerciseSet(userId, trainingPlanExerciseId, loggedSets);

            return result ? Created("", null) : BadRequest();
        }

        [HttpGet("client/{clientId:int}")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> GetClientTrainingPlans(int clientId)
        {
            var trainerId = JwtHelper.GetUserId(Request);

            return Ok(await _trainingPlanLogic.GetClientTrainingPlans(trainerId, clientId));
        }

        [HttpGet("client/{clientId:int}/trainingPlan/{trainingPlanId:int}")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> GetClientTrainingPlans(int clientId, int trainingPlanId)
        {
            var trainerId = JwtHelper.GetUserId(Request);

            return Ok(await _trainingPlanLogic.GetClientTrainingPlanById(trainerId, clientId, trainingPlanId));
        }
    }
}
