SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET XACT_ABORT ON;
GO

IF OBJECT_ID('dbo.Wallets','U') IS NULL
BEGIN
CREATE TABLE dbo.Wallets
(
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    UserId    INT            NOT NULL,
    Balance   DECIMAL(18, 2) NOT NULL DEFAULT 0,
    Currency  CHAR(3)        NOT NULL,
    CreatedAt DATETIME2      NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT FK_Wallets_UserId FOREIGN KEY (UserId) REFERENCES dbo.Users (Id),
    CONSTRAINT UQ_Wallets_UserId_Currency UNIQUE (UserId, Currency),
    CONSTRAINT CHK_Balance_NonNegative CHECK (Balance >= 0)
);
END;