SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE dbo.GetUserByEmail
    @Email NVARCHAR(255)
    AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Id INT, @RetrievedEmail NVARCHAR(255), @PasswordHash NVARCHAR(MAX);

    SELECT TOP 1 
        @Id = Id,
        @RetrievedEmail = Email,
        @PasswordHash = PasswordHash
    FROM dbo.Users
    WHERE Email = @Email;

    IF @Id IS NULL
        THROW 50014, 'User does not exist', 1;

    SELECT 
        @Id AS Id, 
        @RetrievedEmail AS Email, 
        @PasswordHash AS PasswordHash;
END