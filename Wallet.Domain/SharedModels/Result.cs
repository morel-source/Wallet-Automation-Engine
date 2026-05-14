namespace Wallet.Domain.SharedModels;

public readonly record struct Result<T>
{
    public T? Value { get; private init; }

    public ErrorResponse? Error { get; private init; }

    public bool IsSuccess { get; private init; }

    public static Result<T> Success(T value)
    {
        return new Result<T>
        {
            Value = value,
            IsSuccess = true
        };
    }

    public static Result<T> Failure(
        string error,
        DomainErrorCode errorCode = DomainErrorCode.None)
    {
        return new Result<T>
        {
            Error = new ErrorResponse { ErrorCode = errorCode, ErrorMessage = error },
            IsSuccess = false
        };
    }
}

public readonly record struct ErrorResponse
{
    public string ErrorMessage { get; init; }

    public DomainErrorCode ErrorCode { get; init; }
}