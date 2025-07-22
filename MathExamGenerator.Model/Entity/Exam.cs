using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class Exam
{
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? ExamMatrixId { get; set; }

    public string? Name { get; set; }

    public long? Time { get; set; }

    public string? Status { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<ExamDoing> ExamDoings { get; set; } = new List<ExamDoing>();

    public virtual ExamMatrix? ExamMatrix { get; set; }

    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();

    public virtual ICollection<TestStorage> TestStorages { get; set; } = new List<TestStorage>();
}
