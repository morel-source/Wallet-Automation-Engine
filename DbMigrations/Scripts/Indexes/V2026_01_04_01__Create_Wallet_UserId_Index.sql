SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET XACT_ABORT ON;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'IX_Wallets_UserId' 
    AND object_id = OBJECT_ID('dbo.Wallets')
)
BEGIN
    CREATE INDEX IX_Wallets_UserId
    ON dbo.Wallets(UserId);
END;