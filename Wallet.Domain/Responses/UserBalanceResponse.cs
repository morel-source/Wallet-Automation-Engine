namespace Wallet.Domain.Responses;

public readonly record struct UserBalanceResponse
{
    public int WalletId { get; init; }
    public decimal Balance { get; init; }
    public string Currency { get; init; }
}