using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class BookTopic
{
    public Guid Id { get; set; }

    public Guid? BookChapterId { get; set; }

    public string? Name { get; set; }

    public int? TopicNo { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual BookChapter? BookChapter { get; set; }

    public virtual ICollection<MatrixSectionDetail> MatrixSectionDetails { get; set; } = new List<MatrixSectionDetail>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}
