SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE dbo.TransferFunds
    @UserId INT,
    @FromWalletId INT,
    @ToWalletId INT,
    @Amount DECIMAL(18,2)
    AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Wallets WHERE Id = @FromWalletId AND UserId = @UserId)
        THROW 60000, 'Unauthorized wallet access', 1;

    IF @Amount IS NULL OR @Amount <= 0
       THROW 50005, 'Invalid amount', 1;

    IF @FromWalletId = @ToWalletId
       THROW 50007, 'Same wallet', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.Wallets WHERE Id = @ToWalletId)
       THROW 50009, 'Target not found', 1;

    DECLARE @TransactionId INT;

    BEGIN TRANSACTION;

    BEGIN TRY

    DECLARE @Dummy1 INT;
    DECLARE @Dummy2 INT;

    IF @FromWalletId < @ToWalletId
    BEGIN
        SELECT @Dummy1 = 1 FROM dbo.Wallets WITH (UPDLOCK, ROWLOCK) WHERE Id = @FromWalletId;
        SELECT @Dummy2 = 1 FROM dbo.Wallets WITH (UPDLOCK, ROWLOCK) WHERE Id = @ToWalletId;
    END
    ELSE
    BEGIN
        SELECT @Dummy1 = 1 FROM dbo.Wallets WITH (UPDLOCK, ROWLOCK) WHERE Id = @ToWalletId;
        SELECT @Dummy2 = 1 FROM dbo.Wallets WITH (UPDLOCK, ROWLOCK) WHERE Id = @FromWalletId;
    END

    DECLARE @Balance DECIMAL(18,2);

    SELECT @Balance = Balance
    FROM dbo.Wallets
    WHERE Id = @FromWalletId;

    IF @Balance < @Amount
        THROW 50006, 'Insufficient funds', 1;

    INSERT INTO dbo.Transactions (FromWalletId, ToWalletId, Amount, Type)
    VALUES (@FromWalletId, @ToWalletId, @Amount, 3); -- Transfer

    SET @TransactionId = SCOPE_IDENTITY();

    COMMIT TRANSACTION;

    SELECT
        @TransactionId AS TransactionId,
        @FromWalletId AS FromWalletId,
        @ToWalletId AS ToWalletId,
        @Amount AS Amount,
        @Balance - @Amount AS FromBalanceAfter,
        (SELECT Balance FROM dbo.Wallets WHERE Id = @ToWalletId) AS ToBalanceAfter,
        3 AS Type,
        SYSDATETIME() AS CreatedAt;

    END TRY
    BEGIN CATCH
    
    IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
    
            THROW;
    END CATCH
END;