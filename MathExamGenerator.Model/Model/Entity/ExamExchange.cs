using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("ExamExchange")]
public partial class ExamExchange
{
    [Key]
    public Guid Id { get; set; }

    public Guid? TeacherId { get; set; }

    public string? Status { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [InverseProperty("ExamExchange")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    [ForeignKey("TeacherId")]
    [InverseProperty("ExamExchanges")]
    public virtual Teacher? Teacher { get; set; }
}
