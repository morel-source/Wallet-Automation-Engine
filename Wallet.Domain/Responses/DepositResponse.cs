namespace Wallet.Domain.Responses;

public readonly record struct DepositResponse
{
    public int TransactionId { get; init; }
    public int WalletId { get; init; }
    public decimal Amount { get; init; }
    public decimal Balance { get; init; }
    public int Type { get; init; }
    public DateTime CreatedAt { get; init; }
}