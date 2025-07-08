using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("SubjectBook")]
public partial class SubjectBook
{
    [Key]
    public Guid Id { get; set; }

    public Guid? SubjectId { get; set; }

    [StringLength(200)]
    public string? Title { get; set; }

    public string? FileUrl { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [InverseProperty("SubjectBook")]
    public virtual ICollection<BookChapter> BookChapters { get; set; } = new List<BookChapter>();

    [ForeignKey("SubjectId")]
    [InverseProperty("SubjectBooks")]
    public virtual Subject? Subject { get; set; }
}
