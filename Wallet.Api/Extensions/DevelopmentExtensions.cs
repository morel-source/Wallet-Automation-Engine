using Scalar.AspNetCore;

namespace Wallet.Api.Extensions;

public static class DevelopmentExtensions
{
    public static void CheckDevelopmentMode(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;

        app.MapOpenApi();
        app.MapScalarApiReference(configureOptions: options =>
        {
            options.WithTitle("Wallet Automation Engine API").WithTheme(ScalarTheme.DeepSpace);
        });
    }
}