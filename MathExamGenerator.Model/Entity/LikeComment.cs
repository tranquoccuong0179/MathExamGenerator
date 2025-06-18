using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class LikeComment
{
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? CommentId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Comment? Comment { get; set; }
}
