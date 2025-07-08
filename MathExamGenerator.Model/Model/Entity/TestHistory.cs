using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("TestHistory")]
public partial class TestHistory
{
    [Key]
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? ExamId { get; set; }

    public Guid? QuizId { get; set; }

    public double? Grade { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    public TimeOnly? StartAt { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("TestHistories")]
    public virtual Account? Account { get; set; }

    [ForeignKey("ExamId")]
    [InverseProperty("TestHistories")]
    public virtual Exam? Exam { get; set; }

    [InverseProperty("HistoryTest")]
    public virtual ICollection<QuestionHistory> QuestionHistories { get; set; } = new List<QuestionHistory>();

    [ForeignKey("QuizId")]
    [InverseProperty("TestHistories")]
    public virtual Quiz? Quiz { get; set; }
}
