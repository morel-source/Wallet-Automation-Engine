SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE dbo.GetTransactions
    @UserId INT,
    @WalletId INT
    AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Wallets WHERE Id = @WalletId AND UserId = @UserId)
        THROW 50003, 'Wallet not found', 1;

SELECT
    t.Id AS TransactionId,
    t.Amount,
    CASE
        WHEN t.FromWalletId = @WalletId THEN 'OUT'
        ELSE 'IN'
        END AS Direction,
    CASE
        WHEN t.FromWalletId = @WalletId THEN t.ToWalletId
        ELSE t.FromWalletId
        END AS CounterpartyWalletId,
    t.Type,
    t.CreatedAt
    FROM dbo.Transactions t
    WHERE t.FromWalletId = @WalletId OR t.ToWalletId = @WalletId
    ORDER BY t.CreatedAt DESC;
END

