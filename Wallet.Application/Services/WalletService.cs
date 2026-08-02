using Microsoft.Extensions.Logging;
using Wallet.Application.Interfaces;
using Wallet.Domain.Responses;
using Wallet.Domain.SharedModels;
using Wallet.Repository.Interfaces;

namespace Wallet.Application.Services;

public sealed class WalletService(
    ILogger<WalletService> logger,
    IWalletRepository walletRepository,
    ITransactionRepository transactionRepository
) : IWalletService
{
    public async Task<Result<UserBalanceResponse>> GetBalance(int userId, int walletId,
        CancellationToken cancellationToken = default)
    {
        var result = await walletRepository.GetBalance(userId, walletId, cancellationToken);

        if (result.IsSuccess)
            logger.LogInformation("Wallet balance success");
        else
            logger.LogWarning("Wallet balance failed");

        return result;
    }

    public async Task<Result<DepositResponse>> Deposit(int userId, int walletId, decimal amount,
        CancellationToken cancellationToken = default)
    {
        var result = await walletRepository.Deposit(userId, walletId, amount, cancellationToken);

        if (result.IsSuccess)
            logger.LogInformation("Wallet deposit success");
        else
            logger.LogWarning("Wallet deposit failed");

        return result;
    }

    public async Task<Result<WithdrawResponse>> Withdraw(int userId, int walletId, decimal amount,
        CancellationToken cancellationToken = default)
    {
        var result = await walletRepository.Withdraw(userId, walletId, amount, cancellationToken);

        if (result.IsSuccess)
            logger.LogInformation("Wallet withdraw success");
        else
            logger.LogWarning("Wallet withdraw failed");

        return result;
    }

    public async Task<Result<TransferFundsResponse>> TransferFunds(int userId, int fromWalletId, int toWalletId,
        decimal amount, CancellationToken cancellationToken = default)
    {
        var result = await walletRepository.TransferFunds(userId, fromWalletId, toWalletId, amount, cancellationToken);

        if (result.IsSuccess)
            logger.LogInformation("Wallet transfer success");
        else
            logger.LogWarning("Wallet transfer failed");

        return result;
    }

    public async Task<Result<List<TransactionResponse>>> GetTransactions(int userId, int walletId, DateTime? from,
        DateTime? to, int? limit,
        CancellationToken cancellationToken = default)
    {
        var result = await transactionRepository.GetTransactions(userId, walletId, from, to, limit, cancellationToken);

        if (result.IsSuccess)
            logger.LogInformation("Wallet transactions success");
        else
            logger.LogWarning("Wallet transactions failed");

        return result;
    }
}