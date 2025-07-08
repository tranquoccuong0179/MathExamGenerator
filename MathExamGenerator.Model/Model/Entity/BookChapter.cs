using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("BookChapter")]
public partial class BookChapter
{
    [Key]
    public Guid Id { get; set; }

    public Guid? SubjectBookId { get; set; }

    [StringLength(200)]
    public string? Name { get; set; }

    public int? ChapterNo { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [InverseProperty("BookChapter")]
    public virtual ICollection<BookTopic> BookTopics { get; set; } = new List<BookTopic>();

    [InverseProperty("BookChapter")]
    public virtual ICollection<MatrixSectionDetail> MatrixSectionDetails { get; set; } = new List<MatrixSectionDetail>();

    [InverseProperty("BookChapter")]
    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    [ForeignKey("SubjectBookId")]
    [InverseProperty("BookChapters")]
    public virtual SubjectBook? SubjectBook { get; set; }
}
