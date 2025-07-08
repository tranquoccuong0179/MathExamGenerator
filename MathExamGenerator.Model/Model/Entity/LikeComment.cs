using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("LikeComment")]
public partial class LikeComment
{
    [Key]
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? CommentId { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("LikeComments")]
    public virtual Account? Account { get; set; }

    [ForeignKey("CommentId")]
    [InverseProperty("LikeComments")]
    public virtual Comment? Comment { get; set; }
}
