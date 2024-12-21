using Common.Constants;
using System.Security.Claims;

namespace APIGateway.Extensions.Helpers;

public static class UserHelper
{
    public static int EnsureAuthorizedUser(HttpContext context)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException(ErrorMessages.Unauthorized);
        }

        if (!int.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException(ErrorMessages.Unauthorized);
        }

        return userId;
    }
}