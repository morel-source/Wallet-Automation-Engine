using System.Security.Claims;

namespace Wallet.Api.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (value is null)
            throw new UnauthorizedAccessException("Token is missing a user id claim.");

        if (!int.TryParse(value, out var userId))
            throw new UnauthorizedAccessException("Token's user id claim is not a valid integer.");

        return userId;
    }
}