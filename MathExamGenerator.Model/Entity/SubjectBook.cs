using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class SubjectBook
{
    public Guid Id { get; set; }

    public Guid? SubjectId { get; set; }

    public string? Title { get; set; }

    public string? BookImage { get; set; }

    public string? FileUrl { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<BookChapter> BookChapters { get; set; } = new List<BookChapter>();

    public virtual Subject? Subject { get; set; }
}
