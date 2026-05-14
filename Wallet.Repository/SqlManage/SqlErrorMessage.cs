using Microsoft.Data.SqlClient;
using Wallet.Domain.SharedModels;

namespace Wallet.Repository.SqlManage;

public static class SqlErrorMessage
{
    public static (string message, DomainErrorCode code) GetErrorMessageAndCode(this SqlException ex)
    {
        var code = Enum.IsDefined(typeof(DomainErrorCode), ex.Number)
            ? (DomainErrorCode)ex.Number
            : DomainErrorCode.DatabaseError;

        return (DomainErrorMessage.GetMessage(code), code);
    }
}