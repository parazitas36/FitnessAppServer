using DataAccess.Models.SportsClubModels;
using FitnessAppAPI.DTOs.Equipment;
using FitnessAppAPI.DTOs.Facility;
using FitnessAppAPI.Services.Helpers;
using FitnessAppAPI.Services.Interfaces;
using FitnessAppAPI.Services.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessAppAPI.Controllers;

[Route("api/")]
[ApiController]
[Authorize]
public class FacilityController : ControllerBase
{
    private readonly IFacilityLogic _facilityLogic;
    private readonly ISportsClubLogic _sportsClubLogic;

    public FacilityController(IFacilityLogic facilityLogic, ISportsClubLogic sportsClubLogic)
    {
        _facilityLogic = facilityLogic;
        _sportsClubLogic = sportsClubLogic;
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

    [HttpGet("sportsclub/{sportsClubId:int}/facility")]
    public async Task<ActionResult<List<FacilityGetDto>>> GetClubFacilities(int sportsClubId)
    {
        var sportsClub = await _sportsClubLogic.GetSportsClubById(sportsClubId);
        var facilities = await _facilityLogic.GetSportsClubFacilities(sportsClubId);
        return Ok(new {sportsClubName = sportsClub.Name, facilities = facilities});
    }

    [HttpPost("facility/{facilityId:int}/equipment/{equipmentId:int}/{amount:int}")]
    public async Task<ActionResult> AssignEquipmentToFacility(int facilityId, int equipmentId, int amount)
    {
        var isAssigned = await _facilityLogic.AssignEquipmentToFacility(facilityId, equipmentId, amount);

        return isAssigned ? Ok() : NotFound();
    }

    [HttpGet("facility/{facilityId:int}/equipment")]
    public async Task<ActionResult<List<EquipmentGetDto>>> GetFacilityEquipment(int facilityId)
    {
        return Ok(await _facilityLogic.GetFacilityEquipment(facilityId));
    }
}
