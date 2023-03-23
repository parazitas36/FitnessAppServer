using Azure.Core;
using System.IdentityModel.Tokens.Jwt;

namespace FitnessAppAPI.Services.Helpers;

public static class JwtHelper
{
    public static int GetUserId(HttpRequest request)
    {
        var jwt = request.Headers.Authorization.ToString().Split("Bearer ")[1];
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        var userId = int.TryParse(token.Claims.First(x => x.Type == "Id").Value, out int parsedId) ? parsedId : -1;

        return userId;
    }
}
