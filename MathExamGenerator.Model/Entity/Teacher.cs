using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class Teacher
{
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public string? SchoolName { get; set; }

    public Guid? LocationId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<ExamExchange> ExamExchanges { get; set; } = new List<ExamExchange>();

    public virtual Location? Location { get; set; }
}
