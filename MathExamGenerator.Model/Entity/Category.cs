using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class Category
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Grade { get; set; }

    public bool? IsTrue { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
