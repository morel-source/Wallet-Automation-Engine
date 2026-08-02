using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Wallet.Application.Security;

public sealed class JwtProvider(
    ILogger<JwtProvider> logger,
    IOptions<JwtOptions> options
) : IJwtProvider
{
    public string GenerateToken(int userId, string email, int walletId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, "User"),
            new Claim("walletId", walletId.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key));

        var creds = new SigningCredentials(key, algorithm: SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(options.Value.ExpirationDays),
            signingCredentials: creds);

        logger.LogInformation("JWT token generated successfully");
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}