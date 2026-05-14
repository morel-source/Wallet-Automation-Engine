using Microsoft.Data.SqlClient;

namespace Wallet.Repository.SqlManage;

public static class SqlReaderExtensions
{
    public static T Get<T>(this SqlDataReader reader, string column)
    {
        var value = reader[column];

        if (value == DBNull.Value) return default!;

        if (typeof(T).IsEnum)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        return (T)Convert.ChangeType(value, typeof(T));
    }
}