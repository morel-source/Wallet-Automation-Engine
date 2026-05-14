SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE
OR
ALTER PROCEDURE dbo.InsertUser
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(500)
    AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @Email IS NULL OR LTRIM(RTRIM(@Email)) = ''
        THROW 50001, 'Invalid Email', 1;
       
    IF EXISTS (SELECT 1 FROM dbo.Users WHERE Email = @Email)
        THROW 50002, 'User Already Exists', 1;

    BEGIN TRANSACTION;

    BEGIN TRY

    DECLARE @UserId INT;

    INSERT INTO dbo.Users (Email, PasswordHash)
    VALUES (@Email, @PasswordHash);

    SET @UserId = SCOPE_IDENTITY();

    DECLARE @Wallet TABLE (WalletId INT);

    INSERT INTO @Wallet
    EXEC dbo.InsertWallet @UserId = @UserId, @Balance = 0, @Currency = 'ILS';

    COMMIT TRANSACTION;

    SELECT 
        @UserId AS UserId,
       (SELECT TOP 1 WalletId FROM @Wallet) AS WalletId;

    END TRY
    BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

    END CATCH
END;