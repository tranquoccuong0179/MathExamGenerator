using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("Quiz")]
public partial class Quiz
{
    [Key]
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public long? Time { get; set; }

    public int? Quantity { get; set; }

    public Guid? BookTopicId { get; set; }

    public Guid? BookChapterId { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    public Guid? AccountId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Quizzes")]
    public virtual Account? Account { get; set; }

    [ForeignKey("BookChapterId")]
    [InverseProperty("Quizzes")]
    public virtual BookChapter? BookChapter { get; set; }

    [ForeignKey("BookTopicId")]
    [InverseProperty("Quizzes")]
    public virtual BookTopic? BookTopic { get; set; }

    [InverseProperty("Quiz")]
    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();

    [InverseProperty("Quiz")]
    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();

    [InverseProperty("Quiz")]
    public virtual ICollection<TestStorage> TestStorages { get; set; } = new List<TestStorage>();
}
