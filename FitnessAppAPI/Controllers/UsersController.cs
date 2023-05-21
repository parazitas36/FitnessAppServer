using FitnessAppAPI.DTOs.User;
using FitnessAppAPI.Services.Helpers;
using FitnessAppAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessAppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersLogic _usersLogic;
    private readonly IConfiguration _configuration;

    public UsersController(IUsersLogic usersLogic, IConfiguration configuration)
    {
        _usersLogic = usersLogic;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserGetDto>> Login([FromBody] UserLoginDto body)
    {
        UserGetDto? result = await _usersLogic.Login(body.Username, body.Password);

        if (result == null)
        {
            return NotFound();
        }

        var issuer = _configuration.GetValue<string>("Jwt:Issuer");
        var audience = _configuration.GetValue<string>("Jwt:Audience");
        var key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key"));

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", result.Id.ToString()),
                new Claim(ClaimTypes.Role, result.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512),
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

        var jwt = jwtSecurityTokenHandler.WriteToken(token);

        return Ok(new { token = jwt, data = result });
    }

    [HttpPost("register")]
    public async Task<ActionResult<bool>> Register([FromBody] UserRegisterDto body)
    {
        bool result = await _usersLogic.Register(body);

        return result ? Ok() : BadRequest();
    }

    [HttpGet("trainers")]
    [Authorize]
    public async Task<IActionResult> GetTrainers()
    {
        var role = JwtHelper.GetRole(Request);

        if (role == DataAccess.Enumerators.Roles.SportsClubAdmin)
        {
            var sportsClubAdminId = JwtHelper.GetUserId(Request);
            return Ok(await _usersLogic.GetTrainers(sportsClubAdminId));
        }

        return Ok(await _usersLogic.GetTrainers());
    }

    [HttpGet("trainers/{trainerId:int}")]
    [Authorize]
    public async Task<IActionResult> GetTrainer(int trainerId)
    {
        if (JwtHelper.GetRole(Request) == DataAccess.Enumerators.Roles.SportsClubAdmin)
        {
            var sportsClubAdminId = JwtHelper.GetUserId(Request);

            var result = await _usersLogic.GetTrainerInfo(trainerId, sportsClubAdminId);
            return result is null ? NotFound() : Ok(result);
        }
        var trainer = await _usersLogic.GetTrainerInfo(trainerId);
        return trainer is null ? NotFound() : Ok(trainer);
    }

    [HttpPost("reviews")]
    [Authorize]
    public async Task<IActionResult> PostReview([FromBody] ReviewPostDto dto)
    {
        var userId = JwtHelper.GetUserId(Request);
        bool result = await _usersLogic.PostReview(userId, dto);

        return result ? Ok() : BadRequest();
    }

    [HttpGet("clients")]
    [Authorize]
    public async Task<IActionResult> GetClients()
    {
        var trainerId = JwtHelper.GetUserId(Request);

        return Ok(await _usersLogic.GetTrainerClients(trainerId));
    }

    [HttpGet("search/{username}")]
    [Authorize]
    public async Task<IActionResult> FindUsersByUsername(string username)
    {
        return Ok(await _usersLogic.FindUsersByUsername(username));
    }
}
