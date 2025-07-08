using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("Reply")]
public partial class Reply
{
    [Key]
    public Guid Id { get; set; }

    public Guid? CommentId { get; set; }

    public Guid? AccountId { get; set; }

    public bool? IsActive { get; set; }

    public string? Content { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Replies")]
    public virtual Account? Account { get; set; }

    [ForeignKey("CommentId")]
    [InverseProperty("Replies")]
    public virtual Comment? Comment { get; set; }
}
