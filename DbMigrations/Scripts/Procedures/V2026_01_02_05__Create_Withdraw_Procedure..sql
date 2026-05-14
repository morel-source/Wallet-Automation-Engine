SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE dbo.Withdraw
    @UserId INT,
    @WalletId INT,
    @Amount DECIMAL(18,2)
    AS
BEGIN

    SET NOCOUNT ON;
    SET XACT_ABORT ON;
        
    IF NOT EXISTS (SELECT 1 FROM dbo.Wallets WHERE Id = @WalletId AND UserId = @UserId)
        THROW 60000, 'Unauthorized wallet access', 1;

    IF @Amount IS NULL OR @Amount <= 0
        THROW 50005, 'Invalid amount', 1;
       
    DECLARE @Balance DECIMAL(18,2);
            
    SELECT @Balance = Balance 
    FROM dbo.Wallets 
    WHERE Id = @WalletId;
   
    IF @Balance < @Amount 
       THROW 50006, 'Insufficient funds', 1; 

    DECLARE @TransactionId INT;
  
    BEGIN TRANSACTION;

    BEGIN TRY

    DECLARE @Dummy INT;

    SELECT @Dummy = 1
    FROM dbo.Wallets WITH (UPDLOCK, ROWLOCK)
    WHERE Id = @WalletId;

    INSERT INTO dbo.Transactions (FromWalletId,ToWalletId, Amount, Type)
    VALUES (@WalletId,NULL,@Amount,2) --Withdraw

    SET @TransactionId = SCOPE_IDENTITY();

    SELECT
        @TransactionId AS TransactionId,
        @WalletId AS WalletId,
        @Amount AS Amount,
        Balance,
        2 AS Type,
        SYSDATETIME() AS CreatedAt
    FROM dbo.Wallets
    WHERE Id = @WalletId;

    COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        THROW;
    END CATCH
END