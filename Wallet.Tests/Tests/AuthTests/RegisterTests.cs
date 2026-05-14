using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Wallet.Domain.Responses;
using Wallet.Domain.SharedModels;
using Wallet.Tests.Base;
using Wallet.Tests.Infrastructure;

namespace Wallet.Tests.Tests.AuthTests;

public class RegisterTests(TestFactory factory) : ApiTestBase(factory)
{
    [Fact]
    public async Task Register_ShouldCreateUser()
    {
        var res = await Helper.RegisterRequest();
        res.EnsureSuccessStatusCode();

        var data = await res.Content.ReadFromJsonAsync<RegisterResponse>();

        data.Should().NotBeNull();
        data.UserId.Should().BeGreaterThan(0);
        data.WalletId.Should().BeGreaterThan(0);
        data.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Register_DuplicateEmail_ShouldFail()
    {
        var email = Helper.GenerateEmail();

        (await Helper.RegisterRequest(email: email)).EnsureSuccessStatusCode();

        var res = await Helper.RegisterRequest(email: email);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();

        data.Should().NotBeNull();
        data.ErrorCode.Should().Be(DomainErrorCode.UserAlreadyExists);
        data.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        data.ErrorMessage.Should().Be("User already exists.");
    }

    [Fact]
    public async Task Register_InvalidEmail_ShouldFail()
    {
        var res = await Helper.RegisterRequest(email: "abc");
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();

        data.Should().NotBeNull();
        data.ErrorCode.Should().Be(DomainErrorCode.InvalidEmail);
        data.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        data.ErrorMessage.Should().Be("Invalid email");
    }

    [Fact]
    public async Task Register_InvalidPassword_ShouldFail()
    {
        var res = await Helper.RegisterRequest(password: "1234567");
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();

        data.Should().NotBeNull();
        data.ErrorCode.Should().Be(DomainErrorCode.InvalidPassword);
        data.ErrorMessage.Should().NotBeNullOrWhiteSpace();
        data.ErrorMessage.Should().Be("Password is too weak");
    }
}