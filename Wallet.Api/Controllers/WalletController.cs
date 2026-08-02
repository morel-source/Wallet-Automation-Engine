using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Api.Shared.Extensions;
using Wallet.Application.Interfaces;
using Wallet.Domain.Requests;

namespace Wallet.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class WalletController(IWalletService walletService) : ControllerBase
{
    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var res = await walletService.Deposit(userId, request.WalletId, request.Amount, cancellationToken);

        return res.IsSuccess
            ? Ok(res.Value)
            : BadRequest(res.Error);
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var res = await walletService.Withdraw(userId, request.WalletId, request.Amount, cancellationToken);

        return res.IsSuccess
            ? Ok(res.Value)
            : BadRequest(res.Error);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer([FromBody] TransferRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var res = await walletService.TransferFunds(userId, request.FromWalletId, request.ToWalletId, request.Amount,
            cancellationToken);

        return res.IsSuccess
            ? Ok(res.Value)
            : BadRequest(res.Error);
    }

    [HttpGet("{walletId}/balance")]
    public async Task<IActionResult> GetBalance([FromRoute] int walletId,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var res = await walletService.GetBalance(userId, walletId, cancellationToken);

        return res.IsSuccess
            ? Ok(res.Value)
            : BadRequest(res.Error);
    }

    [HttpGet("{walletId}/transactions")]
    public async Task<IActionResult> GetTransactions(
        [FromRoute] int walletId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int? limit,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();

        var res = await walletService.GetTransactions(userId, walletId, from, to, limit, cancellationToken);

        return res.IsSuccess
            ? Ok(res.Value)
            : BadRequest(res.Error);
    }
}