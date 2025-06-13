using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class UserInfo
{
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    public double? Point { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();
}
