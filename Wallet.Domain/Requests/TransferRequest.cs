namespace Wallet.Domain.Requests;

public readonly record struct TransferRequest
{
    public required int FromWalletId { get; init; }
    public required int ToWalletId { get; init; }
    public required decimal Amount { get; init; }
}