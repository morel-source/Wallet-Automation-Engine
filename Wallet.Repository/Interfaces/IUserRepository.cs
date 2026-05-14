using Wallet.Domain.Entities;
using Wallet.Domain.Responses;
using Wallet.Domain.SharedModels;

namespace Wallet.Repository.Interfaces;

public interface IUserRepository
{
    Task<Result<UserResponse>> InsertUser(string email, string passwordHash,
        CancellationToken cancellationToken = default);

    Task<Result<User>> GetUserByEmail(string email,
        CancellationToken cancellationToken = default);
}