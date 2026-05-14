namespace Wallet.Application.Security;

public record JwtOptions(string Key, string Issuer, string Audience)
{
    public const string SectionName = "Jwt";

    public JwtOptions() : this(
        Key: "super-secret-key",
        Issuer: "WalletApi",
        Audience: "WalletApiUsers")
    {
    }
}