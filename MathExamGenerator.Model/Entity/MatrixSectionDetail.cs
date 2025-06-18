using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class MatrixSectionDetail
{
    public Guid Id { get; set; }

    public Guid? MatrixSectionId { get; set; }

    public Guid? BookTopicId { get; set; }

    public Guid? BookChapterId { get; set; }

    public string? Difficulty { get; set; }

    public int? QuestionCount { get; set; }

    public double? ScorePerQuestion { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual BookChapter? BookChapter { get; set; }

    public virtual BookTopic? BookTopic { get; set; }

    public virtual MatrixSection? MatrixSection { get; set; }
}
