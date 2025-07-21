using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class Package
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public int? Point { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<PackageOrder> PackageOrders { get; set; } = new List<PackageOrder>();
}
