SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER TRIGGER dbo.trg_LogWalletUpdate
ON dbo.Wallets
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.AuditLogs (Action, Data, CreatedAt)
    SELECT
        2 AS Action,
            CONCAT(
                'WalletId=', i.Id,
                ', OldBalance=', d.Balance,
                ', NewBalance=', i.Balance
            ) AS Data,
            SYSDATETIME()
    FROM INSERTED i
    INNER JOIN deleted d ON i.Id = d.Id;
END;

