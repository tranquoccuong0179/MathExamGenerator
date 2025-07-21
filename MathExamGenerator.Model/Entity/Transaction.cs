using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class Transaction
{
    public Guid Id { get; set; }

    public Guid? WalletId { get; set; }

    public Guid? DepositId { get; set; }

    public Guid? ExamDoingId { get; set; }

    public Guid? PackageOrderId { get; set; }

    public string? Status { get; set; }

    public string? Type { get; set; }

    public decimal? Amount { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Deposit? Deposit { get; set; }

    public virtual ExamDoing? ExamDoing { get; set; }

    public virtual PackageOrder? PackageOrder { get; set; }

    public virtual Wallet? Wallet { get; set; }
}
