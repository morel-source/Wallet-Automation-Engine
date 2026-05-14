namespace Wallet.Domain.Entities;

public readonly record struct User
{
    public int Id { get; init; }
    public string Email { get; init; }
    public string PasswordHash { get; init; }
}