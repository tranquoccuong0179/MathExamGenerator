using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class Exam
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public double? Grade { get; set; }

    public TimeOnly? Time { get; set; }

    public int? Quantity { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? Minigame { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();

    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();

    public virtual ICollection<TestStorage> TestStorages { get; set; } = new List<TestStorage>();
}
