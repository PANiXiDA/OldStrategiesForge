using Common.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GamePlayService.Extensions.Helpers;

public class JwtHelper
{
    private readonly JwtSettings _jwtSettings;

    public JwtHelper(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public int? ValidateToken(string token)
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

            return userId != null ? int.Parse(userId) : null;
        }
        catch
        {
            return null;
        }
    }
}
