using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("Wallet")]
public partial class Wallet
{
    [Key]
    public Guid Id { get; set; }

    public Guid? AccountId { get; set; }

    public int? Point { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Wallets")]
    public virtual Account? Account { get; set; }

    [InverseProperty("Wallet")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
