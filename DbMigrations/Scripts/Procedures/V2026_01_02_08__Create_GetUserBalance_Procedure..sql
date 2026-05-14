SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE dbo.GetUserBalance
    @UserId INT,
    @WalletId INT
    AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Wallets WHERE UserId = @UserId )
        THROW 60000, 'Unauthorized wallet access', 1;
       
    IF NOT EXISTS ( SELECT 1 FROM dbo.Wallets WHERE Id = @WalletId AND UserId = @UserId )
        THROW 50003, 'Wallet not found', 1;

SELECT
        Id AS WalletId,
        Balance,
        Currency
    FROM dbo.Wallets
    WHERE Id = @WalletId;

END
