using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Wallet.Domain.Enums;
using Wallet.Domain.Responses;
using Wallet.Domain.SharedModels;
using Wallet.Repository.Interfaces;
using Wallet.Repository.SqlManage;

namespace Wallet.Repository.Repositories;

public sealed class TransactionRepository(
    ILogger<TransactionRepository> logger,
    ISqlManageAsync sqlManageAsync
) : ITransactionRepository
{
    public async Task<Result<List<TransactionResponse>>> GetTransactions(
        int userId,
        int walletId,
        DateTime? from,
        DateTime? to,
        int? limit,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await sqlManageAsync.ExecuteListAsync(
                commandText: "dbo.GetTransactions",
                commandType: CommandType.StoredProcedure,
                map: reader => new TransactionResponse
                {
                    TransactionId = reader.Get<int>(column: "TransactionId"),
                    Amount = reader.Get<decimal>(column: "Amount"),
                    Direction = reader.Get<string>(column: "Direction"),
                    CounterpartyWalletId = reader.Get<int>(column: "CounterpartyWalletId"),
                    Type = reader.Get<TransactionType>(column: "Type"),
                    CreatedAt = reader.Get<DateTime>(column: "CreatedAt"),
                },
                cancellationToken: cancellationToken,
                parameters:
                [
                    new SqlParameter("@UserId", SqlDbType.Int) { Value = userId },
                    new SqlParameter("@WalletId", SqlDbType.Int) { Value = walletId },
                    new SqlParameter("@From", SqlDbType.DateTime2) { Value = (object?)from ?? DBNull.Value },
                    new SqlParameter("@To", SqlDbType.DateTime2) { Value = (object?)to ?? DBNull.Value },
                    new SqlParameter("@Limit", SqlDbType.Int) { Value = (object?)limit ?? DBNull.Value }
                ]);

            logger.LogInformation("SQL GetTransactions success");
            return Result<List<TransactionResponse>>.Success(result);
        }
        catch (SqlException ex)
        {
            logger.LogWarning("SQL GetTransactions failed");
            var errorDetails = ex.GetErrorMessageAndCode();
            return Result<List<TransactionResponse>>.Failure(errorDetails.message, errorDetails.code);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: "SQL GetTransactions unexpected error");
            throw;
        }
    }
}