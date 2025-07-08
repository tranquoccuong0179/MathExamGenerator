using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("MatrixSection")]
public partial class MatrixSection
{
    [Key]
    public Guid Id { get; set; }

    public Guid? ExamMatrixId { get; set; }

    [StringLength(200)]
    public string? SectionName { get; set; }

    public int? TotalQuestions { get; set; }

    public double? TotalScore { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [ForeignKey("ExamMatrixId")]
    [InverseProperty("MatrixSections")]
    public virtual ExamMatrix? ExamMatrix { get; set; }

    [InverseProperty("MatrixSection")]
    public virtual ICollection<MatrixSectionDetail> MatrixSectionDetails { get; set; } = new List<MatrixSectionDetail>();
}
