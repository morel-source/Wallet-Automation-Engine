using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Wallet.Domain.Enums;
using Wallet.Domain.SharedModels;
using Wallet.Tests.Base;
using Wallet.Tests.Infrastructure;

namespace Wallet.Tests.Tests.WalletTests;

public class TransferTests(TestFactory factory) : ApiTestBase(factory)
{
    [Fact]
    public async Task Transfer_ShouldMoveMoney()
    {
        var u1 = await Helper.Register();
        var u2 = await Helper.Register();

        Helper.SetToken(u1.Token);

        await Helper.Deposit(u1.WalletId, amount: 500);

        var transfer = await Helper.Transfer(fromWalletId: u1.WalletId, toWalletId: u2.WalletId, amount: 100);
        transfer.Amount.Should().Be(100);
        transfer.FromWalletId.Should().Be(u1.WalletId);
        transfer.ToWalletId.Should().Be(u2.WalletId);
        transfer.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        transfer.ToBalanceAfter.Should().Be(400);
        transfer.TransactionId.Should().BeGreaterThan(0);
        transfer.Type.Should().Be((int)TransactionType.Transfer);

        var b1 = await Helper.GetBalance(walletId: u1.WalletId);

        Helper.SetToken(u2.Token);
        var b2 = await Helper.GetBalance(walletId: u2.WalletId);

        b1.Balance.Should().Be(400);
        b1.WalletId.Should().Be(u1.WalletId);
        b2.Balance.Should().Be(100);
        b2.WalletId.Should().Be(u2.WalletId);
    }

    [Fact]
    public async Task Transfer_InvalidAmount_ShouldFail()
    {
        var u1 = await Helper.Register();
        var u2 = await Helper.Register();

        Helper.SetToken(u1.Token);

        var res = await Helper.TransferRequest(fromWalletId: u1.WalletId, toWalletId: u2.WalletId, amount: -100);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();
        data.Should().NotBeNull();
        data.ErrorCode.Should().Be(DomainErrorCode.InvalidAmount);
        data.ErrorMessage.Should().NotBeNullOrEmpty();
        data.ErrorMessage.Should().Contain("Invalid amount");
    }

    [Fact]
    public async Task Transfer_InvalidTargetWallet_ShouldFail()
    {
        var u1 = await Helper.Register();

        Helper.SetToken(u1.Token);

        await Helper.Deposit(u1.WalletId, 200);

        var res = await Helper.TransferRequest(fromWalletId: u1.WalletId, toWalletId: 7, amount: 100);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();
        data.Should().NotBeNull();
        data.ErrorCode.Should().Be(DomainErrorCode.ToWalletNotFound);
        data.ErrorMessage.Should().NotBeNullOrEmpty();
        data.ErrorMessage.Should().Contain("Destination wallet not found.");
    }

    [Fact]
    public async Task Transfer_InsufficientFunds_ShouldFail()
    {
        var u1 = await Helper.Register();
        var u2 = await Helper.Register();

        Helper.SetToken(u1.Token);

        var res = await Helper.TransferRequest(fromWalletId: u1.WalletId, toWalletId: u2.WalletId, amount: 100);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();
        data.Should().NotBeNull();
        data.ErrorCode.Should().Be(DomainErrorCode.InsufficientFunds);
        data.ErrorMessage.Should().NotBeNullOrEmpty();
        data.ErrorMessage.Should().Contain("Insufficient funds.");
    }

    [Fact]
    public async Task Transfer_FromSameWallet_ShouldFail()
    {
        var u1 = await Helper.Register();

        Helper.SetToken(u1.Token);

        var res = await Helper.TransferRequest(fromWalletId: u1.WalletId, toWalletId: u1.WalletId, amount: 100);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();
        data.Should().NotBeNull();
        data.ErrorCode.Should().Be(DomainErrorCode.CannotTransferFromSameWallet);
        data.ErrorMessage.Should().NotBeNullOrEmpty();
        data.ErrorMessage.Should().Contain("Cannot transfer to the same wallet.");
    }


    [Fact]
    public async Task Transfer_FromAnotherUserWallet_ShouldFail()
    {
        var u1 = await Helper.Register();
        var u2 = await Helper.Register();

        Helper.SetToken(u1.Token);

        var res = await Helper.TransferRequest(fromWalletId: u2.WalletId, toWalletId: u1.WalletId, amount: 100);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();
        data.Should().NotBeNull();
        data.ErrorCode.Should().Be(DomainErrorCode.UnauthorizedWalletAccess);
        data.ErrorMessage.Should().NotBeNullOrEmpty();
        data.ErrorMessage.Should().Contain("You dont have permission to this wallet");
    }
}