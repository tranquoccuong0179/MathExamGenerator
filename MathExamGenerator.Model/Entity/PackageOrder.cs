﻿using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class PackageOrder
{
    public Guid Id { get; set; }

    public Guid? PackageId { get; set; }

    public Guid? AccountId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Package? Package { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
