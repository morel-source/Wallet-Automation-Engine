namespace Wallet.Application.Security;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Key { get; init; } = "super-secret-key";
    public string Issuer { get; init; } = "WalletApi";
    public string Audience { get; init; } = "WalletApiUsers";
    public int ExpirationDays { get; init; } = 7;
}