using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Interfaces;
using Wallet.Domain.Requests;
using Wallet.Domain.SharedModels;

namespace Wallet.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var res = await authService.Register(request.Email, request.Password,
            cancellationToken);

        return res.IsSuccess
            ? Ok(res.Value)
            : BadRequest(res.Error);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var res = await authService.Login(request.Email, request.Password,
            cancellationToken);

        return res.IsSuccess
            ? Ok(res.Value)
            : res.Error?.ErrorCode == DomainErrorCode.Unauthorized
                ? Unauthorized(res.Error)
                : BadRequest(res.Error);
    }
}