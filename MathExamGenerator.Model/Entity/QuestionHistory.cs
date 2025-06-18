using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class QuestionHistory
{
    public Guid Id { get; set; }

    public Guid? HistoryTestId { get; set; }

    public Guid? QuestionId { get; set; }

    public string? Answer { get; set; }

    public string? YourAnswer { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual TestHistory? HistoryTest { get; set; }

    public virtual Question? Question { get; set; }
}
