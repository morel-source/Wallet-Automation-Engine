namespace Wallet.Application.Validators;

public static class PasswordValidator
{
    public static bool IsValid(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        if (password.Length < 8)
            return false;

        var hasUpper = password.Any(char.IsUpper);
        var hasLower = password.Any(char.IsLower);
        var hasDigit = password.Any(char.IsDigit);

        var hasSpecial = password.Any(ch =>
            !char.IsLetterOrDigit(ch));

        return hasUpper
               && hasLower
               && hasDigit
               && hasSpecial;
    }
}