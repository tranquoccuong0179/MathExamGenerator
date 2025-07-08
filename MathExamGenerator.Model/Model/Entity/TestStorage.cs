using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("TestStorage")]
public partial class TestStorage
{
    [Key]
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? ExamId { get; set; }

    public Guid? QuizId { get; set; }

    public bool? Liked { get; set; }

    public bool? Seen { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("TestStorages")]
    public virtual Account? Account { get; set; }

    [ForeignKey("ExamId")]
    [InverseProperty("TestStorages")]
    public virtual Exam? Exam { get; set; }

    [ForeignKey("QuizId")]
    [InverseProperty("TestStorages")]
    public virtual Quiz? Quiz { get; set; }
}
