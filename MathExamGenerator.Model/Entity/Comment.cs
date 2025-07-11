﻿using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class Comment
{
    public Guid Id { get; set; }

    public Guid? QuestionId { get; set; }

    public Guid? AccountId { get; set; }

    public string? Content { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<LikeComment> LikeComments { get; set; } = new List<LikeComment>();

    public virtual Question? Question { get; set; }

    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();
}
