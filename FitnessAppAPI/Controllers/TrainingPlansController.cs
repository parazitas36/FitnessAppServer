﻿using FitnessAppAPI.DTOs.TrainingPlan;
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

            bool trainer = true;

            var result = await _trainingPlanLogic.GetTrainingPlanById(userId, trainingPlanId, trainer);

            return result is null ? NotFound() : Ok(result);
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
    }
}
