using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("Location")]
public partial class Location
{
    [Key]
    public Guid Id { get; set; }

    public string? Name { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [InverseProperty("Location")]
    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
