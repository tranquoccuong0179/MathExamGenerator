using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("Transaction")]
public partial class Transaction
{
    [Key]
    public Guid Id { get; set; }

    public Guid? WalletId { get; set; }

    public Guid? DepositId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? Type { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [ForeignKey("DepositId")]
    [InverseProperty("Transactions")]
    public virtual Deposit? Deposit { get; set; }

    [ForeignKey("WalletId")]
    [InverseProperty("Transactions")]
    public virtual Wallet? Wallet { get; set; }
}
