using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("Teacher")]
[Index("AccountId", Name = "IX_Teacher_AccountId", IsUnique = true)]
public partial class Teacher
{
    [Key]
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    public string? SchoolName { get; set; }

    public Guid? LocationId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Teacher")]
    public virtual Account? Account { get; set; }

    [InverseProperty("Teacher")]
    public virtual ICollection<ExamExchange> ExamExchanges { get; set; } = new List<ExamExchange>();

    [ForeignKey("LocationId")]
    [InverseProperty("Teachers")]
    public virtual Location? Location { get; set; }
}
