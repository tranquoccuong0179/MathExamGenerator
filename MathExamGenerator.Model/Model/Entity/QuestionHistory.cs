using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("QuestionHistory")]
public partial class QuestionHistory
{
    [Key]
    public Guid Id { get; set; }

    public Guid? HistoryTestId { get; set; }

    public Guid? QuestionId { get; set; }

    [StringLength(50)]
    public string? Answer { get; set; }

    [StringLength(50)]
    public string? YourAnswer { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [ForeignKey("HistoryTestId")]
    [InverseProperty("QuestionHistories")]
    public virtual TestHistory? HistoryTest { get; set; }

    [ForeignKey("QuestionId")]
    [InverseProperty("QuestionHistories")]
    public virtual Question? Question { get; set; }
}
