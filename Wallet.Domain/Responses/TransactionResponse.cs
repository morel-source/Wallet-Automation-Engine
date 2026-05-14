using Wallet.Domain.Enums;

namespace Wallet.Domain.Responses;

public readonly record struct TransactionResponse
{
    public int TransactionId { get; init; }
    public decimal Amount { get; init; }
    public string Direction { get; init; }
    public int CounterpartyWalletId { get; init; }
    public TransactionType Type { get; init; }
    public DateTime CreatedAt { get; init; }
}