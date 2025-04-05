using Common.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GamePlayService.Extensions.Helpers;

public static class JwtHelper
{
    private static JwtSettings _jwtSettings = new JwtSettings();

    public static void Configure(JwtSettings settings)
    {
        _jwtSettings = settings;
    }

    public static int GetPlayerIdFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                ClockSkew = TimeSpan.FromHours(1)
            };

            var principal = tokenHandler.ValidateToken(token, parameters, out _);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new Exception("Token validation failed: UserId claim not found.");
            }

            return int.Parse(userId);
        }
        catch (Exception ex)
        {
            throw new Exception("Token validation error", ex);
        }

    }
}
