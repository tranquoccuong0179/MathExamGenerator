using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("MatrixSectionDetail")]
public partial class MatrixSectionDetail
{
    [Key]
    public Guid Id { get; set; }

    public Guid? MatrixSectionId { get; set; }

    public Guid? BookTopicId { get; set; }

    public Guid? BookChapterId { get; set; }

    [StringLength(50)]
    public string? Difficulty { get; set; }

    public int? QuestionCount { get; set; }

    public double? ScorePerQuestion { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [ForeignKey("BookChapterId")]
    [InverseProperty("MatrixSectionDetails")]
    public virtual BookChapter? BookChapter { get; set; }

    [ForeignKey("BookTopicId")]
    [InverseProperty("MatrixSectionDetails")]
    public virtual BookTopic? BookTopic { get; set; }

    [ForeignKey("MatrixSectionId")]
    [InverseProperty("MatrixSectionDetails")]
    public virtual MatrixSection? MatrixSection { get; set; }
}
