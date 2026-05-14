using Microsoft.Extensions.Logging;
using Wallet.Application.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Domain.Responses;
using Wallet.Domain.SharedModels;
using Wallet.Repository.Interfaces;

namespace Wallet.Application.Services;

public sealed class UserService(
    ILogger<UserService> logger,
    IUserRepository userRepository
) : IUserService
{
    public async Task<Result<UserResponse>> InsertUser(string email, string password,
        CancellationToken cancellationToken = default)
    {
        var result = await userRepository.InsertUser(email, password, cancellationToken);

        if (result.IsSuccess)
            logger.LogInformation("User insert success");
        else
            logger.LogWarning("User insert failed");

        return result;
    }

    public async Task<Result<User>> GetUserByEmail(string email, CancellationToken cancellationToken = default)
    {
        var result = await userRepository.GetUserByEmail(email, cancellationToken);

        if (result.IsSuccess)
            logger.LogInformation("User lookup success");
        else
            logger.LogWarning("User lookup failed");

        return result;
    }
}