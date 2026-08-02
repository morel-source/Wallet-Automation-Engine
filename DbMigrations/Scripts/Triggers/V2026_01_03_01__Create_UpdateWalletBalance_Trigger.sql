SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER TRIGGER dbo.trg_UpdateWalletBalance
ON dbo.Transactions
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    ;WITH FromAgg AS(
    SELECT
        FromWalletId as WalletId,
        SUM(Amount) AS TotalAmount
    FROM INSERTED
    WHERE FromWalletId IS NOT NULL
    GROUP BY FromWalletId
    )

    UPDATE w
    SET w.Balance = w.Balance - f.TotalAmount
        FROM dbo.Wallets w
        INNER JOIN FromAgg f
    ON w.Id = f.WalletId


    ;WITH ToAgg AS(
    SELECT
        ToWalletId AS WalletId,
        SUM(Amount) AS TotalAmount
    FROM INSERTED
    WHERE ToWalletId IS NOT NULL
    GROUP BY ToWalletId
    )

    UPDATE w
    SET w.Balance = w.Balance + t.TotalAmount
    FROM dbo.Wallets w
    INNER JOIN ToAgg t ON w.Id = t.WalletId;

    IF EXISTS ( SELECT 1 FROM dbo.Wallets  WHERE Balance < 0)
        THROW 50012, 'Balance cannot be negative', 1;
END;