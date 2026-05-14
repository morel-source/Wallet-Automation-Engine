SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE dbo.InsertWallet
    @UserId INT,
    @Balance DECIMAL (18,2) = 0,
    @Currency CHAR (3)
    AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF EXISTS( SELECT 1 FROM dbo.Wallets WHERE UserId = @UserId AND Currency = @Currency)    
        THROW 50010, 'Already exists wallet with this Currency', 1;
       
    DECLARE @WalletId INT;

    INSERT INTO dbo.Wallets (UserId, Balance, Currency)
    VALUES (@UserId, @Balance, @Currency);

    SET @WalletId = SCOPE_IDENTITY();

    SELECT @WalletId AS WalletId;
END;
