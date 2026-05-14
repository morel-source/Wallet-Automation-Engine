using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Wallet.Domain.Enums;
using Wallet.Domain.SharedModels;
using Wallet.Tests.Base;
using Wallet.Tests.Infrastructure;

namespace Wallet.Tests.Tests.WalletTests;

public class WithdrawTests(TestFactory factory) : ApiTestBase(factory)
{
    [Fact]
    public async Task Withdraw_ShouldDecreaseBalance()
    {
        var user = await Helper.Register();

        Helper.SetToken(user.Token);

        await Helper.Deposit(user.WalletId, 300);

        var deposit = await Helper.Withdraw(walletId: user.WalletId, amount: 100);
        deposit.WalletId.Should().Be(user.WalletId);
        deposit.Amount.Should().Be(100);
        deposit.Balance.Should().Be(200);
        deposit.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        deposit.TransactionId.Should().BeGreaterThan(0);
        deposit.Type.Should().Be((int)TransactionType.Withdraw);

        var balance = await Helper.GetBalance(walletId: user.WalletId);

        balance.Balance.Should().Be(200);
        balance.WalletId.Should().Be(user.WalletId);
        balance.Currency.Should().Be("ILS");
    }

    [Fact]
    public async Task Withdraw_InsufficientFunds_ShouldFail()
    {
        var user = await Helper.Register();

        Helper.SetToken(user.Token);

        var res = await Helper.WithdrawRequest(walletId: user.WalletId, amount: 100);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();
        data.Should().NotBeNull();
        data.ErrorCode.Should().Be(DomainErrorCode.InsufficientFunds);
        data.ErrorMessage.Should().NotBeNullOrEmpty();
        data.ErrorMessage.Should().Contain("Insufficient funds.");
    }

    [Fact]
    public async Task Withdraw_InvalidAmount_ShouldFail()
    {
        var user = await Helper.Register();

        Helper.SetToken(user.Token);

        var res = await Helper.WithdrawRequest(walletId: user.WalletId, amount: -100);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();
        data.Should().NotBeNull();
        data.ErrorCode.Should().Be(DomainErrorCode.InvalidAmount);
        data.ErrorMessage.Should().NotBeNullOrEmpty();
        data.ErrorMessage.Should().Contain("Invalid amount");
    }


    [Fact]
    public async Task Withdraw_FromAnotherUserWallet_ShouldFail()
    {
        var u1 = await Helper.Register();
        var u2 = await Helper.Register();

        Helper.SetToken(u1.Token);

        var res = await Helper.WithdrawRequest(walletId: u2.WalletId, amount: 100);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();
        data.Should().NotBeNull();
        data.ErrorCode.Should().Be(DomainErrorCode.UnauthorizedWalletAccess);
        data.ErrorMessage.Should().NotBeNullOrEmpty();
        data.ErrorMessage.Should().Contain("You dont have permission to this wallet");
    }
}