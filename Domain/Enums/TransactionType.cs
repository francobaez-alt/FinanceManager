using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enums
{
    public enum TransactionType : byte
    {
        Income = 1,
        Expense = 2,
        Transfer = 3
    }
}
