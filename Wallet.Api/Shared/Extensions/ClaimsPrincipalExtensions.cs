using System.Security.Claims;

namespace Wallet.Api.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return value is null ? throw new UnauthorizedAccessException() : int.Parse(value);
    }
}