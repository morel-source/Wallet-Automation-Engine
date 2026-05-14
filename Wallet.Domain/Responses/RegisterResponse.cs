namespace Wallet.Domain.Responses;

public readonly record struct RegisterResponse
{
    public required string Token { get; init; }
    public required int UserId { get; init; }
    public required int WalletId { get; init; }
}