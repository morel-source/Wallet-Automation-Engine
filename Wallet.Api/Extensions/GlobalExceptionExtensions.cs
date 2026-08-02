using Wallet.Api.Middleware;

namespace Wallet.Api.Extensions;

public static class GlobalExceptionExtensions
{
    public static void AddGlobalException(this WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
    }

    public static void UseGlobalException(this WebApplication app)
    {
        app.UseExceptionHandler();
    }
}