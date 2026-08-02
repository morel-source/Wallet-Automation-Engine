namespace Wallet.Api.Extensions;

public static class CorePolicyExtensions
{
    public static void AddCorsPolicy(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "Default", configurePolicy: policy =>
            {
                var allowedOrigins = builder.Configuration.GetSection(key: "Cors:AllowedOrigins").Get<string[]>() ?? [];
                policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
            });
        });
    }

    public static void UseCorsPolicy(this WebApplication app)
    {
        app.UseCors("Default");
    }
}