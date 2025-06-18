using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class MatrixSection
{
    public Guid Id { get; set; }

    public Guid? ExamMatrixId { get; set; }

    public string? SectionName { get; set; }

    public int? TotalQuestions { get; set; }

    public double? TotalScore { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ExamMatrix? ExamMatrix { get; set; }

    public virtual ICollection<MatrixSectionDetail> MatrixSectionDetails { get; set; } = new List<MatrixSectionDetail>();
}
