namespace Wallet.Domain.Requests;

public readonly record struct WithdrawRequest
{
    public required int WalletId { get; init; }
    public required decimal Amount { get; init; }
}