using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class ExamQuestion
{
    public Guid Id { get; set; }

    public Guid? ExamId { get; set; }

    public Guid? QuestionId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Exam? Exam { get; set; }

    public virtual Question? Question { get; set; }
}
