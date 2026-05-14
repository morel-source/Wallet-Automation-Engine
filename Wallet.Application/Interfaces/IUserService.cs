using Wallet.Domain.Entities;
using Wallet.Domain.Responses;
using Wallet.Domain.SharedModels;

namespace Wallet.Application.Interfaces;

public interface IUserService
{
    Task<Result<UserResponse>> InsertUser(string email, string password,
        CancellationToken cancellationToken = default);

    Task<Result<User>> GetUserByEmail(string email,
        CancellationToken cancellationToken = default);
}