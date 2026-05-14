namespace Wallet.Domain.Responses;

public readonly record struct UserResponse
{
    public int UserId { get; init; }
    public int WalletId { get; init; }
}