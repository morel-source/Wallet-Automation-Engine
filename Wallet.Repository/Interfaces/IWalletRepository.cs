using Wallet.Domain.Responses;
using Wallet.Domain.SharedModels;

namespace Wallet.Repository.Interfaces;

public interface IWalletRepository
{
    Task<Result<UserBalanceResponse>> GetBalance(int userId, int walletId,
        CancellationToken cancellationToken = default);

    Task<Result<DepositResponse>> Deposit(int userId, int walletId, decimal amount,
        CancellationToken cancellationToken = default);

    Task<Result<WithdrawResponse>> Withdraw(int userId, int walletId, decimal amount,
        CancellationToken cancellationToken = default);

    Task<Result<TransferFundsResponse>> TransferFunds(int userId, int fromWalletId, int toWalletId, decimal amount,
        CancellationToken cancellationToken = default);
}