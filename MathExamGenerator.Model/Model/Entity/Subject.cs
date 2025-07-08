using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("Subject")]
public partial class Subject
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(20)]
    public string? Code { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [InverseProperty("Subject")]
    public virtual ICollection<ExamMatrix> ExamMatrices { get; set; } = new List<ExamMatrix>();

    [InverseProperty("Subject")]
    public virtual ICollection<SubjectBook> SubjectBooks { get; set; } = new List<SubjectBook>();
}
