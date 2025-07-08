using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("Report")]
public partial class Report
{
    [Key]
    public Guid Id { get; set; }

    public Guid? SendAccountId { get; set; }

    public Guid? ReportedAccountId { get; set; }

    public string? Content { get; set; }

    [StringLength(50)]
    public string? Type { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    [ForeignKey("ReportedAccountId")]
    [InverseProperty("ReportReportedAccounts")]
    public virtual Account? ReportedAccount { get; set; }

    [ForeignKey("SendAccountId")]
    [InverseProperty("ReportSendAccounts")]
    public virtual Account? SendAccount { get; set; }
}
