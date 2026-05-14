using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Wallet.Domain.Responses;
using Wallet.Domain.SharedModels;
using Wallet.Tests.Base;
using Wallet.Tests.Infrastructure;

namespace Wallet.Tests.Tests.AuthTests;

public class LoginTests(TestFactory factory) : ApiTestBase(factory)
{
    [Fact]
    public async Task Login_ShouldReturnToken()
    {
        var email = Helper.GenerateEmail();

        await Helper.Register(email);

        var res = await Helper.LoginRequest(email: email);
        res.EnsureSuccessStatusCode();

        var data = await res.Content.ReadFromJsonAsync<LoginResponse>();

        data.Should().NotBeNull();
        data.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Login_WrongPassword_ShouldFail()
    {
        var email = Helper.GenerateEmail();

        await Helper.Register(email);

        var res = await Helper.LoginRequest(email, password: "My2Password1@");
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();

        data.Should().NotBeNull();
        data.ErrorCode.Should().Be(DomainErrorCode.Unauthorized);
        data.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        data.ErrorMessage.Should().Contain("Invalid credentials");
    }

    [Fact]
    public async Task Login_UserNotExists_ShouldFail()
    {
        var res = await Helper.LoginRequest();
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();

        data.Should().NotBeNull();
        data.ErrorCode.Should().Be(DomainErrorCode.Unauthorized);
        data.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        data.ErrorMessage.Should().Contain("User not exists");
    }

    [Fact]
    public async Task Login_InvalidEmail_ShouldFail()
    {
        var res = await Helper.LoginRequest(email: "abc");
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();

        data.Should().NotBeNull();
        data.ErrorCode.Should().Be(DomainErrorCode.InvalidEmail);
        data.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        data.ErrorMessage.Should().Be("Invalid email");
    }

    [Fact]
    public async Task Login_InvalidPassword_ShouldFail()
    {
        var res = await Helper.LoginRequest(password: "1234567");
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();

        data.Should().NotBeNull();
        data.ErrorCode.Should().Be(DomainErrorCode.InvalidPassword);
        data.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        data.ErrorMessage.Should().Be("Password is too weak");
    }
}