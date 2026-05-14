using FluentAssertions;
using Wallet.Domain.Enums;
using Wallet.Tests.Base;
using Wallet.Tests.Infrastructure;

namespace Wallet.Tests.Tests.WalletTests;

public class TransactionTests(TestFactory factory) : ApiTestBase(factory)
{
    [Fact]
    public async Task Transactions_ShouldContainDeposit()
    {
        var user = await Helper.Register();

        Helper.SetToken(user.Token);

        await Helper.Deposit(walletId: user.WalletId, amount: 100);

        var tx = await Helper.GetTransactions(walletId: user.WalletId);

        tx.Should().Contain(x => x.Type == TransactionType.Deposit);
    }

    [Fact]
    public async Task Transactions_ShouldContainWithdraw()
    {
        var user = await Helper.Register();

        Helper.SetToken(user.Token);

        await Helper.Deposit(walletId: user.WalletId, amount: 300);
        await Helper.Withdraw(walletId: user.WalletId, amount: 100);

        var tx = await Helper.GetTransactions(walletId: user.WalletId);

        tx.Should().Contain(x => x.Type == TransactionType.Withdraw);
    }

    [Fact]
    public async Task Transactions_ShouldContainTransfer()
    {
        var u1 = await Helper.Register();
        var u2 = await Helper.Register();

        Helper.SetToken(u1.Token);

        await Helper.Deposit(walletId: u1.WalletId, amount: 300);

        await Helper.Transfer(fromWalletId: u1.WalletId, toWalletId: u2.WalletId, amount: 100);

        var tx = await Helper.GetTransactions(walletId: u1.WalletId);

        tx.Should().Contain(x => x.Type == TransactionType.Transfer);
    }
}