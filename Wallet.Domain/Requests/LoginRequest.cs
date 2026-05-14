namespace Wallet.Domain.Requests;

public readonly record struct LoginRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}