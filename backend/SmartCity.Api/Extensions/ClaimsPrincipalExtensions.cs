using System.Security.Claims;

namespace SmartCity.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var sub = user.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? user.FindFirstValue(ClaimTypes.Name)
                  ?? user.FindFirstValue("sub");
        if (sub is null || !int.TryParse(sub, out var id))
            throw new UnauthorizedAccessException("Invalid user id claim.");
        return id;
    }
}
