using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("ExamMatrix")]
public partial class ExamMatrix
{
    [Key]
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? SubjectId { get; set; }

    [StringLength(200)]
    public string? Name { get; set; }

    [StringLength(50)]
    public string? Grade { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("ExamMatrices")]
    public virtual Account? Account { get; set; }

    [InverseProperty("ExamMatrix")]
    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    [InverseProperty("ExamMatrix")]
    public virtual ICollection<MatrixSection> MatrixSections { get; set; } = new List<MatrixSection>();

    [ForeignKey("SubjectId")]
    [InverseProperty("ExamMatrices")]
    public virtual Subject? Subject { get; set; }
}
