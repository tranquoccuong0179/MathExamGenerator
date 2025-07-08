using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("Question")]
public partial class Question
{
    [Key]
    public Guid Id { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? ExamExchangeId { get; set; }

    [StringLength(50)]
    public string? Level { get; set; }

    public string? Content { get; set; }

    public string? Solution { get; set; }

    public string? Image { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    public Guid? BookTopicId { get; set; }

    [InverseProperty("Question")]
    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    [ForeignKey("BookTopicId")]
    [InverseProperty("Questions")]
    public virtual BookTopic? BookTopic { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Questions")]
    public virtual Category? Category { get; set; }

    [InverseProperty("Question")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [ForeignKey("ExamExchangeId")]
    [InverseProperty("Questions")]
    public virtual ExamExchange? ExamExchange { get; set; }

    [InverseProperty("Question")]
    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();

    [InverseProperty("Question")]
    public virtual ICollection<QuestionHistory> QuestionHistories { get; set; } = new List<QuestionHistory>();

    [InverseProperty("Question")]
    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}
