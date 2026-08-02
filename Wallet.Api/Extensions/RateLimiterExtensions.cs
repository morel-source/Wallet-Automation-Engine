using Microsoft.AspNetCore.RateLimiting;

namespace Wallet.Api.Extensions;

public static class RateLimiterExtensions
{
    public static void AddRateLimiter(this WebApplicationBuilder builder)
    {
        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddFixedWindowLimiter(policyName: "auth", configureOptions: limiterOptions =>
            {
                limiterOptions.PermitLimit = 100;
                limiterOptions.Window = TimeSpan.FromMinutes(1);
                limiterOptions.QueueLimit = 0;
            });
        });
    }
}