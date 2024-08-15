using System;
using System.Collections.Generic;

namespace RecurrentTransactionMicroservice.Domain.Models;

public partial class RecurrentTransaction
{
    public long Id { get; set; }

    public long? AccountId { get; set; }

    public bool? IsDeposit { get; set; }

    public long? Amount { get; set; }

    public long? Interval { get; set; }
}
