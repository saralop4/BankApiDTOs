﻿using System;
using System.Collections.Generic;

namespace BankAPI.Models;

public partial class Account
{
    public int Id { get; set; }

    public int? AccountType { get; set; }

    public int? ClientId { get; set; }

    public decimal Balance { get; set; }

    public DateTime RegDate { get; set; }

    public virtual AccountType? AccountTypeNavigation { get; set; }

    public virtual ICollection<BankTransaction> BankTransactions { get; set; } = new List<BankTransaction>();

    public virtual Client? Client { get; set; }
}
