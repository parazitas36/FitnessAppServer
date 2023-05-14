using FitnessAppAPI.Services.Helpers;
using FitnessAppAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressLogic _progressLogic;

        public ProgressController(IProgressLogic progressLogic)
        {
            _progressLogic = progressLogic;
        }

        [HttpGet("{trainingPlanId:int}")]
        public async Task<IActionResult> GetTrainingPlanProgress(int trainingPlanId)
        {
            var trainerId = JwtHelper.GetUserId(Request);

            return Ok(await _progressLogic.GetTrainingPlanProgress(trainerId, trainingPlanId));
        }
    }
}
