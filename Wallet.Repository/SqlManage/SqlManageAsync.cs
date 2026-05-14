using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Wallet.Repository.SqlManage;

public sealed class SqlManageAsync(IOptions<SqlProviderOptions> sqlProviderOptions) : ISqlManageAsync
{
    private readonly int _commandTimeout = (int)sqlProviderOptions.Value.CommandTimeout.TotalSeconds;

    private async Task<SqlConnection> CreateConnection(CancellationToken cancellationToken = default)
    {
        var conn = new SqlConnection(sqlProviderOptions.Value.ConnectionStrings);
        await conn.OpenAsync(cancellationToken);
        return conn;
    }

    // Single row
    public async Task<T?> ExecuteReaderAsync<T>(
        string commandText,
        CommandType commandType,
        Func<SqlDataReader, T> map,
        CancellationToken cancellationToken = default,
        params SqlParameter[] parameters)
    {
        await using var connection = await CreateConnection(cancellationToken);
        await using var cmd = new SqlCommand(commandText, connection);
        cmd.CommandType = commandType;
        cmd.CommandTimeout = _commandTimeout;
        cmd.Parameters.AddRange(parameters);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
            return map(reader);

        return default;
    }

    // Multiple rows
    public async Task<List<T>> ExecuteListAsync<T>(
        string commandText,
        CommandType commandType,
        Func<SqlDataReader, T> map,
        CancellationToken cancellationToken = default,
        params SqlParameter[] parameters)
    {
        var result = new List<T>();

        await using var connection = await CreateConnection(cancellationToken);
        await using var cmd = new SqlCommand(commandText, connection);
        cmd.CommandType = commandType;
        cmd.CommandTimeout = _commandTimeout;
        cmd.Parameters.AddRange(parameters);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(map(reader));
        }

        return result;
    }
}