using System;
using System.Collections.Generic;

namespace MathExamGenerator.Model.Entity;

public partial class Account
{
    public Guid Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? AvatarUrl { get; set; }

    public int? FreeTries { get; set; }

    public DateOnly? DailyLoginRewardedAt { get; set; }

    public bool? IsPremium { get; set; }

    public DateTime? PremiumExpireAt { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

    public virtual ICollection<ExamDoing> ExamDoings { get; set; } = new List<ExamDoing>();

    public virtual ICollection<ExamExchange> ExamExchanges { get; set; } = new List<ExamExchange>();

    public virtual ICollection<ExamMatrix> ExamMatrices { get; set; } = new List<ExamMatrix>();

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<LikeComment> LikeComments { get; set; } = new List<LikeComment>();

    public virtual ICollection<PackageOrder> PackageOrders { get; set; } = new List<PackageOrder>();

    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();

    public virtual ICollection<Report> ReportReportedAccounts { get; set; } = new List<Report>();

    public virtual ICollection<Report> ReportSendAccounts { get; set; } = new List<Report>();

    public virtual ICollection<TestStorage> TestStorages { get; set; } = new List<TestStorage>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
