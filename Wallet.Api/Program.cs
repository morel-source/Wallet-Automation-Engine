using System.Text.Json;
using Wallet.Api.Extensions;
using Wallet.Application.Security;

var builder = WebApplication.CreateBuilder(args);

builder.AddGlobalException();
builder.AddCoreServices();
builder.AddJwtAuthentication();
builder.AddCorsPolicy();
builder.AddRateLimiter();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.AddOpenApi();

var app = builder.Build();

app.UseGlobalException();
app.UseHttpsRedirection();
app.UseCorsPolicy();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.CheckDevelopmentMode();
app.MapControllers();

app.MapGet("/", () => "Server is running");

app.Run();