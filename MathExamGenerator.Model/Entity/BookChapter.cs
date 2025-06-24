using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class BookChapter
{
    public Guid Id { get; set; }

    public Guid? SubjectBookId { get; set; }

    public string? Name { get; set; }

    public int? ChapterNo { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<BookTopic> BookTopics { get; set; } = new List<BookTopic>();

    public virtual ICollection<MatrixSectionDetail> MatrixSectionDetails { get; set; } = new List<MatrixSectionDetail>();

    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    public virtual SubjectBook? SubjectBook { get; set; }
}
