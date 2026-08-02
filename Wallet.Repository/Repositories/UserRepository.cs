using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Wallet.Domain.Entities;
using Wallet.Domain.Responses;
using Wallet.Domain.SharedModels;
using Wallet.Repository.Interfaces;
using Wallet.Repository.SqlManage;

namespace Wallet.Repository.Repositories;

public sealed class UserRepository(
    ILogger<UserRepository> logger,
    ISqlManageAsync sqlManageAsync
) : IUserRepository
{
    public async Task<Result<UserResponse>> InsertUser(string email, string passwordHash,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await sqlManageAsync.ExecuteReaderAsync(
                commandText: "dbo.InsertUser",
                commandType: CommandType.StoredProcedure,
                map: reader => new UserResponse
                {
                    UserId = reader.Get<int>(column: "UserId"),
                    WalletId = reader.Get<int>(column: "WalletId"),
                },
                cancellationToken: cancellationToken,
                parameters:
                [
                    new SqlParameter("@Email", SqlDbType.NVarChar, 255) { Value = email },
                    new SqlParameter("@PasswordHash", SqlDbType.NVarChar) { Value = passwordHash }
                ]);

            logger.LogInformation("SQL InsertUser success");
            return Result<UserResponse>.Success(result);
        }
        catch (SqlException ex)
        {
            logger.LogWarning("SQL InsertUser failed");
            var errorDetails = ex.GetErrorMessageAndCode();
            return Result<UserResponse>.Failure(errorDetails.message, errorDetails.code);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: "SQL InsertUser unexpected error");
            throw;
        }
    }

    public async Task<Result<User>> GetUserByEmail(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await sqlManageAsync.ExecuteReaderAsync(
                commandText: "dbo.GetUserByEmail",
                commandType: CommandType.StoredProcedure,
                map: reader => new User
                {
                    Id = reader.Get<int>(column: "Id"),
                    Email = reader.Get<string>(column: "Email"),
                    PasswordHash = reader.Get<string>(column: "PasswordHash"),
                    WalletId = reader.Get<int>(column: "WalletId")
                },
                cancellationToken: cancellationToken,
                parameters: new SqlParameter("@Email", email));

            logger.LogInformation("SQL GetUserByEmail success");
            return Result<User>.Success(result);
        }
        catch (SqlException ex)
        {
            logger.LogWarning("SQL GetUserByEmail failed");
            var errorDetails = ex.GetErrorMessageAndCode();
            return Result<User>.Failure(errorDetails.message, errorDetails.code);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, message: "SQL GetUserByEmail unexpected error");
            throw;
        }
    }
}