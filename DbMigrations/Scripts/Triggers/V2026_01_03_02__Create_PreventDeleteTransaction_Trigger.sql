SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER TRIGGER dbo.trg_PreventDeleteTransaction
ON dbo.Transactions
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    THROW 50011, 'Deleting transactions is not allowed. Data is immutable.', 1;
END;