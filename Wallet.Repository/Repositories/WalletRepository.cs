using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Wallet.Domain.Responses;
using Wallet.Domain.SharedModels;
using Wallet.Repository.Interfaces;
using Wallet.Repository.SqlManage;

namespace Wallet.Repository.Repositories;

public sealed class WalletRepository(
    ILogger<WalletRepository> logger,
    ISqlManageAsync sqlManageAsync
) : IWalletRepository
{
    public async Task<Result<UserBalanceResponse>> GetBalance(int userId, int walletId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await sqlManageAsync.ExecuteReaderAsync(
                commandText: "dbo.GetUserBalance",
                commandType: CommandType.StoredProcedure,
                map: reader => new UserBalanceResponse
                {
                    WalletId = reader.Get<int>(column: "WalletId"),
                    Balance = reader.Get<decimal>(column: "Balance"),
                    Currency = reader.Get<string>(column: "Currency"),
                },
                cancellationToken: cancellationToken,
                parameters:
                [
                    new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                    new SqlParameter("@WalletId", SqlDbType.Int) { Value = walletId }
                ]);

            logger.LogInformation("SQL GetBalance success");
            return Result<UserBalanceResponse>.Success(result);
        }
        catch (SqlException ex)
        {
            logger.LogWarning("SQL GetBalance failed");
            var errorDetails = ex.GetErrorMessageAndCode();
            return Result<UserBalanceResponse>.Failure(errorDetails.message, errorDetails.code);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: "SQL GetBalance unexpected error");
            throw;
        }
    }

    public async Task<Result<DepositResponse>> Deposit(int userId, int walletId, decimal amount,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await sqlManageAsync.ExecuteReaderAsync(
                commandText: "dbo.Deposit",
                commandType: CommandType.StoredProcedure,
                map: reader => new DepositResponse
                {
                    TransactionId = reader.Get<int>(column: "TransactionId"),
                    WalletId = reader.Get<int>(column: "WalletId"),
                    Amount = reader.Get<decimal>(column: "Amount"),
                    Balance = reader.Get<decimal>(column: "Balance"),
                    Type = reader.Get<int>(column: "Type"),
                    CreatedAt = reader.Get<DateTime>(column: "CreatedAt"),
                },
                cancellationToken: cancellationToken,
                parameters:
                [
                    new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                    new SqlParameter("@WalletId", SqlDbType.Int) { Value = walletId },
                    new SqlParameter("@Amount", SqlDbType.Decimal) { Value = amount }
                ]);

            logger.LogInformation("SQL Deposit success");
            return Result<DepositResponse>.Success(result);
        }
        catch (SqlException ex)
        {
            logger.LogWarning("SQL Deposit failed");
            var errorDetails = ex.GetErrorMessageAndCode();
            return Result<DepositResponse>.Failure(errorDetails.message, errorDetails.code);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: "SQL Deposit unexpected error");
            throw;
        }
    }

    public async Task<Result<WithdrawResponse>> Withdraw(int userId, int walletId, decimal amount,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await sqlManageAsync.ExecuteReaderAsync(
                commandText: "dbo.Withdraw",
                commandType: CommandType.StoredProcedure,
                map: reader => new WithdrawResponse
                {
                    TransactionId = reader.Get<int>(column: "TransactionId"),
                    WalletId = reader.Get<int>(column: "WalletId"),
                    Amount = reader.Get<decimal>(column: "Amount"),
                    Balance = reader.Get<decimal>(column: "Balance"),
                    Type = reader.Get<int>(column: "Type"),
                    CreatedAt = reader.Get<DateTime>(column: "CreatedAt"),
                },
                cancellationToken: cancellationToken,
                parameters:
                [
                    new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                    new SqlParameter("@WalletId", SqlDbType.Int) { Value = walletId },
                    new SqlParameter("@Amount", SqlDbType.Decimal) { Value = amount }
                ]);

            logger.LogInformation("SQL Withdraw success");
            return Result<WithdrawResponse>.Success(result);
        }
        catch (SqlException ex)
        {
            logger.LogWarning("SQL Withdraw failed");
            var errorDetails = ex.GetErrorMessageAndCode();
            return Result<WithdrawResponse>.Failure(errorDetails.message, errorDetails.code);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: "SQL Withdraw unexpected error");
            throw;
        }
    }

    public async Task<Result<TransferFundsResponse>> TransferFunds(int userId, int fromWalletId, int toWalletId,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await sqlManageAsync.ExecuteReaderAsync(
                commandText: "dbo.TransferFunds",
                commandType: CommandType.StoredProcedure,
                map: reader => new TransferFundsResponse
                {
                    TransactionId = reader.Get<int>(column: "TransactionId"),
                    FromWalletId = reader.Get<int>(column: "FromWalletId"),
                    ToWalletId = reader.Get<int>(column: "ToWalletId"),
                    Amount = reader.Get<decimal>(column: "Amount"),
                    ToBalanceAfter = reader.Get<decimal>(column: "ToBalanceAfter"),
                    Type = reader.Get<int>(column: "Type"),
                    CreatedAt = reader.Get<DateTime>(column: "CreatedAt"),
                },
                cancellationToken: cancellationToken,
                parameters:
                [
                    new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                    new SqlParameter("@FromWalletId", SqlDbType.Int) { Value = fromWalletId },
                    new SqlParameter("@ToWalletId", SqlDbType.Int) { Value = toWalletId },
                    new SqlParameter("@Amount", SqlDbType.Decimal) { Value = amount }
                ]);

            logger.LogInformation("SQL TransferFunds success");
            return Result<TransferFundsResponse>.Success(result);
        }
        catch (SqlException ex)
        {
            logger.LogWarning("SQL TransferFunds failed");
            var errorDetails = ex.GetErrorMessageAndCode();
            return Result<TransferFundsResponse>.Failure(errorDetails.message, errorDetails.code);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: "SQL TransferFunds unexpected error");
            throw;
        }
    }
}