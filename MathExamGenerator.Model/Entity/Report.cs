using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class Report
{
    public Guid Id { get; set; }

    public Guid? SendAccountId { get; set; }

    public Guid? ReportedAccountId { get; set; }

    public string? Content { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account? ReportedAccount { get; set; }

    public virtual Account? SendAccount { get; set; }
}
