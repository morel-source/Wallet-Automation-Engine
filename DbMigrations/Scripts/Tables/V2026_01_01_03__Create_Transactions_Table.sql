SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET XACT_ABORT ON;
GO

IF OBJECT_ID('dbo.Transactions','U') IS NULL
BEGIN
CREATE TABLE dbo.Transactions
(
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    FromWalletId INT NULL,
    ToWalletId   INT NULL,
    Amount       DECIMAL(18, 2) NOT NULL,
    Type         TINYINT        NOT NULL,
    CreatedAt    DATETIME2      NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT FK_Transactions_FromWalletId FOREIGN KEY (FromWalletId) REFERENCES dbo.Wallets (Id),
    CONSTRAINT FK_Transactions_ToWalletId FOREIGN KEY (ToWalletId) REFERENCES dbo.Wallets (Id),
    CONSTRAINT CHK_AmountPositive CHECK (Amount > 0),
    CONSTRAINT CHK_Amount_Positive CHECK (Amount > 0),
    CONSTRAINT CHK_Type_Valid CHECK (Type IN (1, 2, 3)), -- Deposit(1),Withdraw(2),Transfer(3)         
    CONSTRAINT CHK_Transaction_Type_Logic CHECK
        (
        (Type = 1 AND ToWalletId IS NOT NULL AND FromWalletId IS NULL) OR
        (Type = 2 AND FromWalletId IS NOT NULL AND ToWalletId IS NULL) OR
        (Type = 3 AND FromWalletId IS NOT NULL AND ToWalletId IS NOT NULL)
        )
);
END;