namespace Wallet.Domain.SharedModels;

public static class DomainErrorMessage
{
    public static string GetMessage(DomainErrorCode code)
    {
        var message = DomainErrors.ResourceManager.GetString(code.ToString());
        return message ?? "Unexpected error occurred.";
    }
}