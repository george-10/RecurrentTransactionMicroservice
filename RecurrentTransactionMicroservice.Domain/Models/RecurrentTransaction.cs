﻿using System;
using System.Collections.Generic;

namespace RecurrentTransactionMicroservice.Domain.Models;

public partial class RecurrentTransaction
{
    public long Id { get; set; }

    public long? AccountId { get; set; }

    public bool? IsDeposit { get; set; }

    public long? Amount { get; set; }

    public long? Interval { get; set; }

    public DateTime? LastTransactionDate { get; set; }

    public string? BranchId { get; set; }
    public bool IsTransactionDue()
    {
        if (!LastTransactionDate.HasValue)
            return true;

        return (DateTime.UtcNow - LastTransactionDate.Value).TotalSeconds >= Interval;
    }
}
