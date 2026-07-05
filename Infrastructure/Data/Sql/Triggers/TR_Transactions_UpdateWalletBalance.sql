CREATE TRIGGER TR_Transactions_UpdateWalletBalance
ON TRANSACTIONS
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -------------------------------------------------------
    -- Si es un UPDATE que no modifica el saldo,
    -- salir inmediatamente.
    -------------------------------------------------------
    IF EXISTS (SELECT 1 FROM inserted)
       AND EXISTS (SELECT 1 FROM deleted)
       AND NOT (
            UPDATE(WalletId)
         OR UPDATE(Amount)
         OR UPDATE(Type)
       )
    BEGIN
        RETURN;
    END;

    -------------------------------------------------------
    -- Calcular el impacto sobre cada Wallet
    -------------------------------------------------------
    ;WITH BalanceChanges AS
    (
        -------------------------------------------------------
        -- Revertir el efecto de las filas anteriores
        -------------------------------------------------------
        SELECT
            WalletId,
            CASE
                WHEN Type = 1 THEN -Amount      -- Era Income
                WHEN Type = 2 THEN Amount       -- Era Expense
            END AS Delta
        FROM deleted

        UNION ALL

        -------------------------------------------------------
        -- Aplicar el efecto de las nuevas filas
        -------------------------------------------------------
        SELECT
            WalletId,
            CASE
                WHEN Type = 1 THEN Amount       -- Nuevo Income
                WHEN Type = 2 THEN -Amount      -- Nuevo Expense
            END AS Delta
        FROM inserted
    ),
    WalletTotals AS
    (
        SELECT
            WalletId,
            SUM(Delta) AS TotalDelta
        FROM BalanceChanges
        GROUP BY WalletId
    )

    -------------------------------------------------------
    -- Actualizar balances
    -------------------------------------------------------
    UPDATE W
    SET W.Balance = W.Balance + WT.TotalDelta
    FROM WALLETS W
    INNER JOIN WalletTotals WT
        ON WT.WalletId = W.Id;

    -------------------------------------------------------
    -- Validar que ningún balance quede negativo
    -------------------------------------------------------
    IF EXISTS
    (
        SELECT 1
        FROM WALLETS
        WHERE Balance < 0
    )
    BEGIN
        RAISERROR ('Insufficient balance.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END;
GO