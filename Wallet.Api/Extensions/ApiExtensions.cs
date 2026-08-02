using Wallet.Application.Interfaces;
using Wallet.Application.Security;
using Wallet.Application.Services;
using Wallet.Repository.Interfaces;
using Wallet.Repository.Repositories;
using Wallet.Repository.SqlManage;

namespace Wallet.Api.Extensions;

public static class ApiExtensions
{
    extension(WebApplicationBuilder builder)
    {
        public void AddCoreServices()
        {
            builder.Services.Configure<SqlProviderOptions>(builder.Configuration.GetSection(key: "SqlProviderOptions"));
            builder.AddRepositories();
            builder.AddServices();
        }

        private void AddServices()
        {
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IWalletService, WalletService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
        }

        private void AddRepositories()
        {
            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddSingleton<IWalletRepository, WalletRepository>();
            builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();
            builder.Services.AddSingleton<ISqlManageAsync, SqlManageAsync>();
        }
    }
}