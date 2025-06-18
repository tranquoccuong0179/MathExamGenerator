using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class QuizQuestion
{
    public Guid Id { get; set; }

    public Guid? QuestionId { get; set; }

    public Guid? QuizId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Question? Question { get; set; }

    public virtual Quiz? Quiz { get; set; }
}
