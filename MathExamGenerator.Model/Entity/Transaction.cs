using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class Transaction
{
    public Guid Id { get; set; }

    public Guid? WalletId { get; set; }

    public Guid? DepositId { get; set; }

    public decimal? Amount { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Deposit? Deposit { get; set; }

    public virtual Wallet? Wallet { get; set; }
}
