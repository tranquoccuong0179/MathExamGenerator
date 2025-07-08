using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("Deposit")]
public partial class Deposit
{
    [Key]
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Code { get; set; }

    [StringLength(100)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Deposits")]
    public virtual Account? Account { get; set; }

    [InverseProperty("Deposit")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
