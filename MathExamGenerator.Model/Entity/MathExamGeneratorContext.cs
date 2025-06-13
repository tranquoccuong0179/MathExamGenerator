using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MathExamGenerator.Model.Entity;

public partial class MathExamGeneratorContext : DbContext
{
    public MathExamGeneratorContext()
    {
    }

    public MathExamGeneratorContext(DbContextOptions<MathExamGeneratorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Deposit> Deposits { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<ExamExchange> ExamExchanges { get; set; }

    public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<Reply> Replies { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TestHistory> TestHistories { get; set; }

    public virtual DbSet<TestStorage> TestStorages { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }
    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString("DefautDB")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07FEE4FF29");

            entity.ToTable("Account");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(15);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Role).HasMaxLength(15);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Answer__3214EC0741389F39");

            entity.ToTable("Answer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_Answer_Question");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07D80E9BB2");

            entity.ToTable("Category");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC07E855F0C7");

            entity.ToTable("Comment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Question).WithMany(p => p.Comments)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_Comment_Question");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Comment_User");
        });

        modelBuilder.Entity<Deposit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Deposit__3214EC0799A84DDA");

            entity.ToTable("Deposit");

            entity.HasIndex(e => e.Code, "UQ__Deposit__A25C5AA71570D6DE").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Deposits)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Deposit_Account");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exam__3214EC07480B34CF");

            entity.ToTable("Exam");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<ExamExchange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExamExch__3214EC07417F4852");

            entity.ToTable("ExamExchange");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Teacher).WithMany(p => p.ExamExchanges)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK_ExamExchange_Teacher");
        });

        modelBuilder.Entity<ExamQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExamQues__3214EC07904EC3A9");

            entity.ToTable("ExamQuestion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamQuestions)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("FK_ExamQuestion_Exam");

            entity.HasOne(d => d.Question).WithMany(p => p.ExamQuestions)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_ExamQuestion_Question");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Like__3214EC07DBFEE6F6");

            entity.ToTable("Like");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Likes)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Like_Account");

            entity.HasOne(d => d.Comment).WithMany(p => p.Likes)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK_Like_Comment");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Province__3214EC07DC189C4A");

            entity.ToTable("Province");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC0730842E0F");

            entity.ToTable("Question");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Level).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Questions)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Question_Category");

            entity.HasOne(d => d.ExamExchange).WithMany(p => p.Questions)
                .HasForeignKey(d => d.ExamExchangeId)
                .HasConstraintName("FK_Question_ExamExchange");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quiz__3214EC074C497D22");

            entity.ToTable("Quiz");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reply__3214EC07371F2310");

            entity.ToTable("Reply");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Comment).WithMany(p => p.Replies)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK_Reply_Comment");

            entity.HasOne(d => d.User).WithMany(p => p.Replies)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Reply_User");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Report__3214EC07814E5E08");

            entity.ToTable("Report");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.ReportedAccount).WithMany(p => p.ReportReportedAccounts)
                .HasForeignKey(d => d.ReportedAccountId)
                .HasConstraintName("FK_Report_ReportedAccount");

            entity.HasOne(d => d.SendAccount).WithMany(p => p.ReportSendAccounts)
                .HasForeignKey(d => d.SendAccountId)
                .HasConstraintName("FK_Report_SentAccount");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teacher__3214EC07AE616428");

            entity.ToTable("Teacher");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Teacher_Account");

            entity.HasOne(d => d.Province).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK_Teacher_Province");
        });

        modelBuilder.Entity<TestHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestHist__3214EC073CDC0319");

            entity.ToTable("TestHistory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_TestHistory_Account");

            entity.HasOne(d => d.Exam).WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestHistory_Exam");

            entity.HasOne(d => d.Quiz).WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.QuizId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestHistory_Quiz");
        });

        modelBuilder.Entity<TestStorage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestStor__3214EC077882F390");

            entity.ToTable("TestStorage");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.TestStorages)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_TestStorage_Account");

            entity.HasOne(d => d.Exam).WithMany(p => p.TestStorages)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestStorage_Exam");

            entity.HasOne(d => d.Quiz).WithMany(p => p.TestStorages)
                .HasForeignKey(d => d.QuizId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestStorage_Quiz");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC075A8BE734");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Deposit).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.DepositId)
                .HasConstraintName("FK_Transaction_Deposit");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK_Transaction_Wallet");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserInfo__3214EC078D8E9342");

            entity.ToTable("UserInfo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.UserInfos)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_UserInfo_Account");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC0777FB08BF");

            entity.ToTable("Wallet");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Wallet_Account");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
