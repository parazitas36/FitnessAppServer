using DataAccess.Enumerators;
using FitnessAppAPI.DTOs.Forms;
using FitnessAppAPI.Services.Helpers;
using FitnessAppAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FormsController : ControllerBase
    {
        private readonly IFormsLogic _formsLogic;

        public FormsController(IFormsLogic formsLogic)
        {
            _formsLogic = formsLogic;
        }

        [HttpGet("bodymeasurements")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetBodyMeasureMents() 
        {
            var userId = JwtHelper.GetUserId(Request);

            return Ok(await _formsLogic.GetBodyMeasurements(userId));
        }

        [HttpPost("bodymeasurements")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> PostBodyMeasurements([FromForm] BodyMeasurementsPostDto dto)
        {
            var userId = JwtHelper.GetUserId(Request);

            bool result = await _formsLogic.PostMeasurements(userId, dto);

            return result ? Created(nameof(BodyMeasurementsPostDto), null) : BadRequest();
        }

        [HttpGet("trainer/joboffers")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> GetJobOffers()
        {
            var trainerId = JwtHelper.GetUserId(Request);

            return Ok(await _formsLogic.GetJobOffers(trainerId));
        }

        [HttpGet("sportsclub/joboffers")]
        [Authorize(Roles = "SportsClubAdmin")]
        public async Task<IActionResult> GetSportsClubJobOffers()
        {
            var sportsClubAdminId = JwtHelper.GetUserId(Request);

            return Ok(await _formsLogic.GetSportsClubsJobOffers(sportsClubAdminId));
        }

        [HttpPost("joboffers")]
        [Authorize(Roles = "SportsClubAdmin")]
        public async Task<IActionResult> PostJobOffer([FromBody] JobOfferPostDto dto)
        {
            var userId = JwtHelper.GetUserId(Request);

            bool result = await _formsLogic.PostJobOffer(userId, dto);

            return result ? Created(nameof(JobOfferPostDto), null) : BadRequest(); 
        }

        [HttpGet("trainer/trainerjobform")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> GetTrainerJobForm()
        {
            var userId = JwtHelper.GetUserId(Request);

            return Ok(await _formsLogic.GetTrainerJobForms(userId));
        }

        [HttpGet("trainerjobform")]
        public async Task<IActionResult> GetTrainersJobForm()
        {
            return Ok(await _formsLogic.GetTrainersJobForms());
        }

        [HttpPost("trainerjobform")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> PostTrainerJobForm([FromBody] TrainerJobFormPostDto dto)
        {
            var userId = JwtHelper.GetUserId(Request);

            bool result = await _formsLogic.PostTrainerJobForm(userId, dto);

            return result ? Created(nameof(TrainerJobFormPostDto), null) : BadRequest();
        }

        [HttpGet("trainingplanforms")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> GetTrainingPlanForms()
        {
            var userId = JwtHelper.GetUserId(Request);
            return Ok(await _formsLogic.GetTrainingPlanForms(userId));
        }

        [HttpGet("user/trainingplanforms")]
        public async Task<IActionResult> GetUsersTrainingPlanForms()
        {
            var userId = JwtHelper.GetUserId(Request);

            return Ok(await _formsLogic.GetUsersTrainingPlanForms(userId));
        }

        [HttpPost("trainingplanforms")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> PostTrainingPlanForms([FromBody] TrainingPlanFormPostDto dto)
        {
            var userId = JwtHelper.GetUserId(Request);

            bool result = await _formsLogic.PostTrainingPlanForm(userId, dto);

            return result ? Created(nameof(TrainingPlanFormPostDto), null) : BadRequest();
        }

        [HttpGet("trainingplanoffers/user")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserTrainingPlanOffers()
        {
            var userId = JwtHelper.GetUserId(Request);
            var list = await _formsLogic.GetTrainingPlanOffers(userId);

            return Ok(list);
        }

        [HttpGet("trainingplanoffers/trainer")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> GetTrainerTrainingPlanOffers()
        {
            var userId = JwtHelper.GetUserId(Request);
            var list = await _formsLogic.GetTrainingPlanOffers(userId, true);

            return Ok(list);
        }

        [HttpPatch("trainingplanoffers/{trainingPlanId:int}/{status}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateTrainingPlanOffer(int trainingPlanId, string status)
        {
            OfferStatus offerStatus;

            if (!Enum.TryParse(status, out offerStatus))
            {
                return BadRequest();
            }

            var userId = JwtHelper.GetUserId(Request);
            bool result = await _formsLogic.UpdateTrainingPlanOffer(userId, trainingPlanId, offerStatus);

            return result ? Ok() : BadRequest();
        }

        [HttpDelete("trainingplanoffers/{trainingPlanId:int}")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> DeleteTrainingPlanOffer(int trainingPlanId)
        {
            var userId = JwtHelper.GetUserId(Request);
            bool result = await _formsLogic.DeleteTrainingPlanOffer(userId, trainingPlanId);

            return result ? NoContent() : BadRequest();
        }

        [HttpPost("trainingplanoffers")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> PostTrainingPlanOffer([FromBody] TrainingPlanOfferPostDto dto)
        {
            var userId = JwtHelper.GetUserId(Request);
            bool result = await _formsLogic.PostTrainingPlanOffer(userId, dto);

            return result ? Created(nameof(TrainingPlanOfferPostDto), null) : BadRequest();
        }
    }
}
