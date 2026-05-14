using System.Net;
using FluentAssertions;
using Wallet.Tests.Base;
using Wallet.Tests.Infrastructure;

namespace Wallet.Tests.Tests.AuthorizationTests;

public class JwtAuthorizationTests(TestFactory factory) : ApiTestBase(factory)
{
    [Fact]
    public async Task Deposit_WithoutToken_ShouldFail()
    {
        var res = await Helper.DepositRequest(walletId: 1, amount: 100);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Withdraw_WithoutToken_ShouldFail()
    {
        var res = await Helper.WithdrawRequest(walletId: 1, amount: 100);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Transfer_WithoutToken_ShouldFail()
    {
        var res = await Helper.TransferRequest(fromWalletId: 1, toWalletId: 2, amount: 100);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetBalance_WithoutToken_ShouldFail()
    {
        var res = await Helper.GetBalanceRequest(walletId: 1);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetTransactions_WithoutToken_ShouldFail()
    {
        var res = await Helper.GetTransactionsRequest(walletId: 1);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}