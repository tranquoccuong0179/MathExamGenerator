using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class Comment
{
    public Guid Id { get; set; }

    public Guid? QuestionId { get; set; }

    public Guid? UserId { get; set; }

    public string? Content { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public virtual Question? Question { get; set; }

    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();

    public virtual UserInfo? User { get; set; }
}
