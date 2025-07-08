using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("ExamQuestion")]
public partial class ExamQuestion
{
    [Key]
    public Guid Id { get; set; }

    public Guid? ExamId { get; set; }

    public Guid? QuestionId { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [ForeignKey("ExamId")]
    [InverseProperty("ExamQuestions")]
    public virtual Exam? Exam { get; set; }

    [ForeignKey("QuestionId")]
    [InverseProperty("ExamQuestions")]
    public virtual Question? Question { get; set; }
}
