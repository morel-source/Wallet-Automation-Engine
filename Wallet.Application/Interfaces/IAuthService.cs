using Wallet.Domain.Responses;
using Wallet.Domain.SharedModels;

namespace Wallet.Application.Interfaces;

public interface IAuthService
{
    Task<Result<RegisterResponse>> Register(string email, string password,
        CancellationToken cancellationToken = default);

    Task<Result<LoginResponse>> Login(string email, string password,
        CancellationToken cancellationToken = default);
}