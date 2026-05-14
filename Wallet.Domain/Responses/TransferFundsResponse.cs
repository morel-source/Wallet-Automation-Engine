namespace Wallet.Domain.Responses;

public readonly record struct TransferFundsResponse
{
    public int TransactionId { get; init; }
    public int FromWalletId { get; init; }
    public int ToWalletId { get; init; }
    public decimal Amount { get; init; }
    public decimal ToBalanceAfter { get; init; }
    public int Type { get; init; }
    public DateTime CreatedAt { get; init; }
}