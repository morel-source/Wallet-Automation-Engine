namespace Wallet.Domain.SharedModels;

public enum DomainErrorCode
{
    None = 0,
    InvalidEmail = 50001,
    UserAlreadyExists = 50002,
    WalletNotFound = 50003,
    WalletNotBelongToUser = 50004,
    InvalidAmount = 50005,
    InsufficientFunds = 50006,
    CannotTransferFromSameWallet = 50007,
    FromWalletNotFound = 50008,
    ToWalletNotFound = 50009,
    WalletCurrencyAlreadyExists = 50010,
    TransactionDeletionNotAllowed = 50011,
    BalanceCannotBeNegative = 50012,
    InvalidPassword = 50013,
    UserNotExist = 50014,
    UnauthorizedWalletAccess = 60000,
    Unauthorized = 70000,
    DatabaseError = 99999
}