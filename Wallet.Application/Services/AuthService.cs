using Microsoft.Extensions.Logging;
using Wallet.Application.Interfaces;
using Wallet.Application.Security;
using Wallet.Application.Validators;
using Wallet.Domain.Responses;
using Wallet.Domain.SharedModels;

namespace Wallet.Application.Services;

public sealed class AuthService(
    ILogger<AuthService> logger,
    IUserService userService,
    IJwtProvider jwtProvider
) : IAuthService
{
    public async Task<Result<RegisterResponse>> Register(string email, string password,
        CancellationToken cancellationToken = default)
    {
        if (!EmailValidator.IsValid(email))
        {
            logger.LogWarning("Register validation failed (email)");
            return Result<RegisterResponse>.Failure(error: "Invalid email", DomainErrorCode.InvalidEmail);
        }

        if (!PasswordValidator.IsValid(password))
        {
            logger.LogWarning("Register validation failed (password)");
            return Result<RegisterResponse>.Failure(error: "Password is too weak", DomainErrorCode.InvalidPassword);
        }

        var hashPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var user = await userService.InsertUser(email, hashPassword, cancellationToken);

        if (user is { IsSuccess: false, Error: { } error })
        {
            logger.LogWarning("Register user creation failed");
            return Result<RegisterResponse>.Failure(error.ErrorMessage, error.ErrorCode);
        }

        var token = jwtProvider.GenerateToken(user.Value.UserId, email);

        logger.LogInformation("Register complete successfully");

        return Result<RegisterResponse>.Success(new RegisterResponse
        {
            Token = token,
            UserId = user.Value.UserId,
            WalletId = user.Value.WalletId
        });
    }

    public async Task<Result<LoginResponse>> Login(string email, string password,
        CancellationToken cancellationToken = default)
    {
        if (!EmailValidator.IsValid(email))
        {
            logger.LogWarning("Login validation failed (email)");
            return Result<LoginResponse>.Failure(error: "Invalid email", DomainErrorCode.InvalidEmail);
        }

        if (!PasswordValidator.IsValid(password))
        {
            logger.LogWarning("Register validation failed (password)");
            return Result<LoginResponse>.Failure(error: "Password is too weak", DomainErrorCode.InvalidPassword);
        }

        var user = await userService.GetUserByEmail(email, cancellationToken);

        if (user is { IsSuccess: false, Error: { } error })
        {
            logger.LogWarning("Get user failed");
            return Result<LoginResponse>.Failure(error.ErrorMessage,
                error.ErrorCode == DomainErrorCode.UserNotExist ? DomainErrorCode.Unauthorized : error.ErrorCode);
        }

        var valid = BCrypt.Net.BCrypt.Verify(password, user.Value.PasswordHash);

        if (!valid)
        {
            logger.LogWarning("Login invalid credentials");
            return Result<LoginResponse>.Failure("Invalid credentials", DomainErrorCode.Unauthorized);
        }

        var token = jwtProvider.GenerateToken(user.Value.Id, user.Value.Email);

        logger.LogInformation("Login completed");

        return Result<LoginResponse>.Success(new LoginResponse { Token = token });
    }
}