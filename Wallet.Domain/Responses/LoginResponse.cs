namespace Wallet.Domain.Responses;

public readonly record struct LoginResponse
{
    public string Token { get; init; }
}