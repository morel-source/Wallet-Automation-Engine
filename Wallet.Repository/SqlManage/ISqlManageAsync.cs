using System.Data;
using Microsoft.Data.SqlClient;

namespace Wallet.Repository.SqlManage;

public interface ISqlManageAsync
{
    public Task<T?> ExecuteReaderAsync<T>(
        string commandText,
        CommandType commandType,
        Func<SqlDataReader, T> map,
        CancellationToken cancellationToken = default,
        params SqlParameter[] parameters);

    public Task<List<T>> ExecuteListAsync<T>(
        string commandText,
        CommandType commandType,
        Func<SqlDataReader, T> map,
        CancellationToken cancellationToken = default,
        params SqlParameter[] parameters);
}