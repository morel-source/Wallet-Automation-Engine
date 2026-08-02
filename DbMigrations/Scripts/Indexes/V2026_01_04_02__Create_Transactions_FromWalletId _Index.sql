SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET XACT_ABORT ON;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_Transactions_FromWallet' 
    AND object_id = OBJECT_ID('dbo.Transactions')
)
BEGIN
    CREATE INDEX IX_Transactions_FromWallet
    ON dbo.Transactions(FromWalletId);
END;