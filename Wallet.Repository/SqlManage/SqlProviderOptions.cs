namespace Wallet.Repository.SqlManage;

public record SqlProviderOptions(
    string ConnectionStrings,
    TimeSpan CommandTimeout
)
{
    public SqlProviderOptions() : this(
        ConnectionStrings: string.Empty,
        CommandTimeout: TimeSpan.FromSeconds(60))
    {
    }
}