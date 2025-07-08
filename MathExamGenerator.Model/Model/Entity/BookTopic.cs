using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("BookTopic")]
public partial class BookTopic
{
    [Key]
    public Guid Id { get; set; }

    public Guid? BookChapterId { get; set; }

    [StringLength(200)]
    public string? Name { get; set; }

    public int? TopicNo { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [ForeignKey("BookChapterId")]
    [InverseProperty("BookTopics")]
    public virtual BookChapter? BookChapter { get; set; }

    [InverseProperty("BookTopic")]
    public virtual ICollection<MatrixSectionDetail> MatrixSectionDetails { get; set; } = new List<MatrixSectionDetail>();

    [InverseProperty("BookTopic")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    [InverseProperty("BookTopic")]
    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}
