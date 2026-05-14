namespace Wallet.Domain.Requests;

public readonly record struct DepositRequest
{
    public required int WalletId { get; init; }
    public required decimal Amount { get; init; }
}