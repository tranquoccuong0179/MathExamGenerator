using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class Subject
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<ExamMatrix> ExamMatrices { get; set; } = new List<ExamMatrix>();

    public virtual ICollection<SubjectBook> SubjectBooks { get; set; } = new List<SubjectBook>();
}
