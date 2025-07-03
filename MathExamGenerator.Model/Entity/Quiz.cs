using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class Quiz
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public long? Time { get; set; }

    public int? Quantity { get; set; }

    public Guid? BookTopicId { get; set; }

    public Guid? BookChapterId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public Guid? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual BookChapter? BookChapter { get; set; }

    public virtual BookTopic? BookTopic { get; set; }

    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();

    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();

    public virtual ICollection<TestStorage> TestStorages { get; set; } = new List<TestStorage>();
}
