﻿using DataAccess.DatabaseContext;
using DataAccess.Models.SportsClubModels;
using FitnessAppAPI.DTOs.Equipment;
using FitnessAppAPI.DTOs.SportsClub;
using FitnessAppAPI.Services.Helpers;
using FitnessAppAPI.Services.Interfaces;
using FitnessAppAPI.Services.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FitnessAppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SportsClubController : ControllerBase
{
    private readonly ISportsClubLogic _sportsClubLogic;

    public SportsClubController(ISportsClubLogic sportsClubLogic)
    {
        _sportsClubLogic = sportsClubLogic;
    }

    [HttpGet("usersportsclub")]
    [Authorize(Roles = "SportsClubAdmin")]
    public async Task<IActionResult> GetUserSportsClub()
    {
        var userId = JwtHelper.GetUserId(Request);

        if (userId == -1)
        {
            return Forbid();
        }

        var result = await _sportsClubLogic.GetUserSportsClub(userId);

        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "SportsClubAdmin")]
    public async Task<IActionResult> CreateSportsClub([FromForm] SportsClubPostDto body)
    {
        var userId = JwtHelper.GetUserId(Request);

        if (userId == -1)
        {
            return Forbid();
        }

        var result = await _sportsClubLogic.CreateSportsClub(userId, body);

        return result is null ? Conflict() : Created(nameof(SportsClub), result);
    }

    [HttpGet("all/{page:int}")]
    public async Task<IActionResult> GetAllSportsClubs(int page)
    {
        return Ok(await _sportsClubLogic.GetAllSportsClubs(page));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSportsClubById(int id)
    {
        var result = await _sportsClubLogic.GetSportsClubById(id, false);

        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:int}/full")]
    public async Task<IActionResult> GetSportsClubByIdFull(int id)
    {
        var result = await _sportsClubLogic.GetSportsClubById(id, true);

        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{sportsClubId:int}/subscriptions")]
    [Authorize(Roles = "SportsClubAdmin")]
    public async Task<IActionResult> CreateSubscription(int sportsClubId, [FromBody] SubscriptionPostDto body)
    {
        var userId = JwtHelper.GetUserId(Request);

        if (userId == -1)
        {
            return Forbid();
        }

        bool isCreated = await _sportsClubLogic.CreateSubscription(body, sportsClubId, userId);

        return isCreated ? Created(nameof(SportsClub), null) : BadRequest();
    }

    [HttpGet("{sportsClubId:int}/subscriptions")]
    public async Task<IActionResult> GetAllClubSubscriptions(int sportsClubId)
    {
        var sportsClub = await _sportsClubLogic.GetSportsClubById(sportsClubId);
        var subscriptios = await _sportsClubLogic.GetAllSubscriptions(sportsClubId);
        return Ok(new { sportsClubName = sportsClub?.Name, subscriptions = subscriptios});
    }

    [HttpGet("{sportsClubId:int}/equipment")]
    public async Task<ActionResult<Equipment>> GetSportsClubEquipment(int sportsClubId)
    {
        var equipment = await _sportsClubLogic.GetSportsClubEquipment(sportsClubId);

        return Ok(equipment);
    }

    [HttpGet("trainer/equipment")]
    public async Task<IActionResult> GetTrainerEquipment()
    {
        var trainerId = JwtHelper.GetUserId(Request);
        var result = await _sportsClubLogic.GetTrainersEquipment(trainerId);

        return Ok(result);
    }

    [HttpPost("{sportsClubId:int}/equipment")]
    [Authorize(Roles = "SportsClubAdmin")]
    public async Task<ActionResult<Equipment>> CreateEquipment([FromForm] EquipmentPostDto body, int sportsClubId)
    {
        var sportsClub = await _sportsClubLogic.GetSportsClubById(sportsClubId);

        if (sportsClub == null) { return NotFound(); }

        var userId = JwtHelper.GetUserId(Request);

        if (userId != sportsClub.OwnerId) { return Forbid(); }

        var equipment = await _sportsClubLogic.CreateEquipment(sportsClubId, body);

        return equipment == null ? BadRequest() : Created(nameof(Equipment), equipment);
    }

    [HttpGet("{sportsClubId:int}/trainers")]
    public async Task<IActionResult> GetSportsClubTrainers(int sportsClubId)
    {
        return Ok(await _sportsClubLogic.GetSportsClubTrainers(sportsClubId));
    }
}
