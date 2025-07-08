using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("Comment")]
public partial class Comment
{
    [Key]
    public Guid Id { get; set; }

    public Guid? QuestionId { get; set; }

    public Guid? AccountId { get; set; }

    public string? Content { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Comments")]
    public virtual Account? Account { get; set; }

    [InverseProperty("Comment")]
    public virtual ICollection<LikeComment> LikeComments { get; set; } = new List<LikeComment>();

    [ForeignKey("QuestionId")]
    [InverseProperty("Comments")]
    public virtual Question? Question { get; set; }

    [InverseProperty("Comment")]
    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();
}
