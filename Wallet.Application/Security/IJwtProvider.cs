namespace Wallet.Application.Security;

public interface IJwtProvider
{
    string GenerateToken(int userId, string email);
}