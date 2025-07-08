using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("Exam")]
public partial class Exam
{
    [Key]
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public long? Time { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    public Guid? ExamMatrixId { get; set; }

    public Guid? AccountId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Exams")]
    public virtual Account? Account { get; set; }

    [ForeignKey("ExamMatrixId")]
    [InverseProperty("Exams")]
    public virtual ExamMatrix? ExamMatrix { get; set; }

    [InverseProperty("Exam")]
    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();

    [InverseProperty("Exam")]
    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();

    [InverseProperty("Exam")]
    public virtual ICollection<TestStorage> TestStorages { get; set; } = new List<TestStorage>();
}
