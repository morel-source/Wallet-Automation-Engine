using System.Net.Http.Headers;
using System.Net.Http.Json;
using Wallet.Domain.Requests;
using Wallet.Domain.Responses;

namespace Wallet.Tests.Helpers;

public class TestHelper(HttpClient client)
{
    public string GenerateEmail() => $"{Guid.NewGuid()}@test.com";
    private const string Password = "MyPassword1@";

    public async Task<HttpResponseMessage> RegisterRequest(string? email = null, string? password = null,
        CancellationToken cancellation = default)
    {
        return await client.PostAsJsonAsync(requestUri: "/api/auth/register", new RegisterRequest
        {
            Email = email ?? GenerateEmail(),
            Password = password ?? Password
        }, cancellationToken: cancellation);
    }

    public async Task<HttpResponseMessage> LoginRequest(string? email = null, string? password = null,
        CancellationToken cancellationToken = default)
    {
        return await client.PostAsJsonAsync(requestUri: "/api/auth/login", new LoginRequest
        {
            Email = email ?? GenerateEmail(),
            Password = password ?? Password
        }, cancellationToken: cancellationToken);
    }

    public async Task<HttpResponseMessage> DepositRequest(int walletId, decimal amount,
        CancellationToken cancellationToken = default)
    {
        return await client.PostAsJsonAsync(requestUri: "/api/wallet/deposit", new DepositRequest
        {
            WalletId = walletId,
            Amount = amount
        }, cancellationToken: cancellationToken);
    }

    public async Task<HttpResponseMessage> WithdrawRequest(int walletId, decimal amount,
        CancellationToken cancellationToken = default)
    {
        return await client.PostAsJsonAsync(requestUri: "/api/wallet/withdraw", new WithdrawRequest
        {
            WalletId = walletId,
            Amount = amount
        }, cancellationToken: cancellationToken);
    }

    public async Task<HttpResponseMessage> TransferRequest(int fromWalletId, int toWalletId, decimal amount,
        CancellationToken cancellationToken = default)
    {
        return await client.PostAsJsonAsync(requestUri: "/api/wallet/transfer", new TransferRequest
        {
            FromWalletId = fromWalletId,
            ToWalletId = toWalletId,
            Amount = amount
        }, cancellationToken: cancellationToken);
    }

    public async Task<HttpResponseMessage> GetBalanceRequest(int walletId,
        CancellationToken cancellationToken = default)
    {
        return await client.GetAsync(requestUri: $"/api/wallet/{walletId}/balance",
            cancellationToken: cancellationToken);
    }

    public async Task<HttpResponseMessage> GetTransactionsRequest(int walletId,
        CancellationToken cancellationToken = default)
    {
        return await client.GetAsync(requestUri: $"/api/wallet/{walletId}/transactions",
            cancellationToken: cancellationToken);
    }

    public void SetToken(string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<RegisterResponse> Register(string? email = null, string? password = null)
    {
        var res = await RegisterRequest(email, password);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<RegisterResponse>();
    }

    public async Task<DepositResponse> Deposit(int walletId, decimal amount)
    {
        var res = await DepositRequest(walletId, amount);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<DepositResponse>();
    }

    public async Task<WithdrawResponse> Withdraw(int walletId, decimal amount)
    {
        var res = await WithdrawRequest(walletId, amount);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<WithdrawResponse>();
    }

    public async Task<TransferFundsResponse> Transfer(int fromWalletId, int toWalletId, decimal amount)
    {
        var res = await TransferRequest(fromWalletId, toWalletId, amount);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<TransferFundsResponse>();
    }

    public async Task<UserBalanceResponse> GetBalance(int walletId)
    {
        var res = await GetBalanceRequest(walletId);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<UserBalanceResponse>();
    }

    public async Task<List<TransactionResponse>> GetTransactions(int walletId)
    {
        var res = await GetTransactionsRequest(walletId);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<List<TransactionResponse>>() ?? [];
    }
}