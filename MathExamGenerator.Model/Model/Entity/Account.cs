using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

[Table("Account")]
public partial class Account
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string? UserName { get; set; }

    [StringLength(50)]
    public string? Password { get; set; }

    [StringLength(15)]
    public string? Role { get; set; }

    [StringLength(100)]
    public string? FullName { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(15)]
    public string? Phone { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [StringLength(15)]
    public string? Gender { get; set; }

    public string? AvatarUrl { get; set; }

    public int? QuizFree { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteAt { get; set; }

    public DateOnly? DailyLoginRewardedAt { get; set; }

    public bool? IsPremium { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PremiumExpireAt { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [InverseProperty("Account")]
    public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

    [InverseProperty("Account")]
    public virtual ICollection<ExamMatrix> ExamMatrices { get; set; } = new List<ExamMatrix>();

    [InverseProperty("Account")]
    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    [InverseProperty("Account")]
    public virtual ICollection<LikeComment> LikeComments { get; set; } = new List<LikeComment>();

    [InverseProperty("Account")]
    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    [InverseProperty("Account")]
    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();

    [InverseProperty("ReportedAccount")]
    public virtual ICollection<Report> ReportReportedAccounts { get; set; } = new List<Report>();

    [InverseProperty("SendAccount")]
    public virtual ICollection<Report> ReportSendAccounts { get; set; } = new List<Report>();

    [InverseProperty("Account")]
    public virtual Teacher? Teacher { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();

    [InverseProperty("Account")]
    public virtual ICollection<TestStorage> TestStorages { get; set; } = new List<TestStorage>();

    [InverseProperty("Account")]
    public virtual ICollection<UserInfo> UserInfos { get; set; } = new List<UserInfo>();

    [InverseProperty("Account")]
    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
