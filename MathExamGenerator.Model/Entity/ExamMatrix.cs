using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class ExamMatrix
{
    public Guid Id { get; set; }

    public Guid? SubjectId { get; set; }

    public string? Name { get; set; }

    public string? Grade { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<MatrixSection> MatrixSections { get; set; } = new List<MatrixSection>();

    public virtual Subject? Subject { get; set; }
}
