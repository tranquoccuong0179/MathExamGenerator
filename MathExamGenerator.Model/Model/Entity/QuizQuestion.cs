using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("QuizQuestion")]
public partial class QuizQuestion
{
    [Key]
    public Guid Id { get; set; }

    public Guid? QuestionId { get; set; }

    public Guid? QuizId { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [ForeignKey("QuestionId")]
    [InverseProperty("QuizQuestions")]
    public virtual Question? Question { get; set; }

    [ForeignKey("QuizId")]
    [InverseProperty("QuizQuestions")]
    public virtual Quiz? Quiz { get; set; }
}
