using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class Question
{
    public Guid Id { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? ExamExchangeId { get; set; }

    public string? Level { get; set; }

    public string? Content { get; set; }

    public string? Solution { get; set; }

    public string? Image { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ExamExchange? ExamExchange { get; set; }

    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();
}
