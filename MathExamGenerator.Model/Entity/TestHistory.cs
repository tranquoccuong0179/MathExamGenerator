﻿using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class TestHistory
{
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? ExamId { get; set; }

    public Guid? QuizId { get; set; }

    public double? Grade { get; set; }

    public string? Status { get; set; }

    public TimeSpan? StartAt { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Exam? Exam { get; set; }

    public virtual ICollection<QuestionHistory> QuestionHistories { get; set; } = new List<QuestionHistory>();

    public virtual Quiz? Quiz { get; set; }
}
