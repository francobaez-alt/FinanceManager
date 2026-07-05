CREATE TRIGGER TR_Transactions_Audit
ON TRANSACTIONS
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    ----------------------------------------------------------
    -- INSERT
    ----------------------------------------------------------
    INSERT INTO TRANSACTION_HISTORY
    (
        TransactionId,
        PreviousAmount,
        NewAmount,
        PreviousDescription,
        NewDescription,
        ModifiedAt
    )
    SELECT
        I.Id,
        NULL,
        I.Amount,
        NULL,
        I.Description,
        GETDATE()
    FROM inserted I
    LEFT JOIN deleted D
        ON I.Id = D.Id
    WHERE D.Id IS NULL;

    ----------------------------------------------------------
    -- UPDATE
    ----------------------------------------------------------
    INSERT INTO TRANSACTION_HISTORY
    (
        TransactionId,
        PreviousAmount,
        NewAmount,
        PreviousDescription,
        NewDescription,
        ModifiedAt
    )
    SELECT
        I.Id,
        D.Amount,
        I.Amount,
        D.Description,
        I.Description,
        GETDATE()
    FROM inserted I
    INNER JOIN deleted D
        ON I.Id = D.Id;

    ----------------------------------------------------------
    -- DELETE
    ----------------------------------------------------------
    INSERT INTO TRANSACTION_HISTORY
    (
        TransactionId,
        PreviousAmount,
        NewAmount,
        PreviousDescription,
        NewDescription,
        ModifiedAt
    )
    SELECT
        D.Id,
        D.Amount,
        NULL,
        D.Description,
        NULL,
        GETDATE()
    FROM deleted D
    LEFT JOIN inserted I
        ON I.Id = D.Id
    WHERE I.Id IS NULL;
END;
GO
