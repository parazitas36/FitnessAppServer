using DataAccess.Models.SportsClubModels;
using FitnessAppAPI.DTOs.Equipment;
using FitnessAppAPI.DTOs.Facility;
using FitnessAppAPI.Services.Helpers;
using FitnessAppAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessAppAPI.Controllers;

[Route("api/")]
[ApiController]
[Authorize]
public class FacilityController : ControllerBase
{
    private readonly IFacilityLogic _facilityLogic;

    public FacilityController(IFacilityLogic facilityLogic)
    {
        _facilityLogic = facilityLogic;
    }

    [HttpPost("sportsclub/{sportsClubId:int}/facility")]
    [Authorize(Roles = "SportsClubAdmin")]
    public async Task<ActionResult> CreateFacility(int sportsClubId, [FromBody] FacilityPostDto body)
    {
        var userId = JwtHelper.GetUserId(Request);

        if (userId == -1)
        {
            return Forbid();
        }

        var isCreated = await _facilityLogic.CreateFacility(body, sportsClubId, userId);

        return isCreated ? Created(nameof(Facility), null) : BadRequest();
    }

    [HttpPost("equipment")]
    public async Task<ActionResult<Equipment>> CreateEquipment([FromBody] EquipmentPostDto body)
    {
        var equipment = await _facilityLogic.CreateEquipment(body);

        return equipment == null ? BadRequest() : Created(nameof(Equipment), equipment);
    }

    [HttpGet("sportsclub/{sportsClubId:int}/facility")]
    public async Task<ActionResult<List<FacilityGetDto>>> GetClubFacilities(int sportsClubId)
    {
        return Ok(await _facilityLogic.GetSportsClubFacilities(sportsClubId));
    }

    [HttpPost("facility/{facilityId:int}/equipment/{equipmentId:int}/{amount:int}")]
    public async Task<ActionResult> AssignEquipmentToFacility(int facilityId, int equipmentId, int amount)
    {
        var isAssigned = await _facilityLogic.AssignEquipmentToFacility(facilityId, equipmentId, amount);

        return isAssigned ? Ok() : NotFound();
    }

    [HttpGet("facility/{facilityId:int}/equipment")]
    public async Task<ActionResult> GetFacilityEquipment(int facilityId)
    {
        return Ok(await _facilityLogic.GetFacilityEquipment(facilityId));
    }
}
