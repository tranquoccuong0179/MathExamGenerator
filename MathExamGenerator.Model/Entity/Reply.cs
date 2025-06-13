using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class Reply
{
    public Guid Id { get; set; }

    public Guid? CommentId { get; set; }

    public Guid? UserId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Comment? Comment { get; set; }

    public virtual UserInfo? User { get; set; }
}
