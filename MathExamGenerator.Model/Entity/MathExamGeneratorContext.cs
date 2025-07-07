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

    public virtual DbSet<BookChapter> BookChapters { get; set; }

    public virtual DbSet<BookTopic> BookTopics { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Deposit> Deposits { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<ExamExchange> ExamExchanges { get; set; }

    public virtual DbSet<ExamMatrix> ExamMatrices { get; set; }

    public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }

    public virtual DbSet<LikeComment> LikeComments { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<MatrixSection> MatrixSections { get; set; }

    public virtual DbSet<MatrixSectionDetail> MatrixSectionDetails { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionHistory> QuestionHistories { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<QuizQuestion> QuizQuestions { get; set; }

    public virtual DbSet<Reply> Replies { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<SubjectBook> SubjectBooks { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC074096ED73");

            entity.ToTable("Account");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(15);
            entity.Property(e => e.IsPremium).HasDefaultValue(false);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.PremiumExpireAt).HasColumnType("datetime");
            entity.Property(e => e.Role).HasMaxLength(15);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Answer__3214EC07B5E20FD8");

            entity.ToTable("Answer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_Answer.QuestionId");
        });

        modelBuilder.Entity<BookChapter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookChap__3214EC07A89A62ED");

            entity.ToTable("BookChapter");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.SubjectBook).WithMany(p => p.BookChapters)
                .HasForeignKey(d => d.SubjectBookId)
                .HasConstraintName("FK_BookChapter.SubjectBookId");
        });

        modelBuilder.Entity<BookTopic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookTopi__3214EC071C6DD74E");

            entity.ToTable("BookTopic");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.BookChapter).WithMany(p => p.BookTopics)
                .HasForeignKey(d => d.BookChapterId)
                .HasConstraintName("FK_BookTopic.BookChapterId");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07976D58D8");

            entity.ToTable("Category");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Grade).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC0748C2919F");

            entity.ToTable("Comment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Comments)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Comment.AccountId");

            entity.HasOne(d => d.Question).WithMany(p => p.Comments)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_Comment.QuestionId");
        });

        modelBuilder.Entity<Deposit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Deposit__3214EC07F4F27651");

            entity.ToTable("Deposit");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Deposits)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Deposit.AccountId");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exam__3214EC075E16BD22");

            entity.ToTable("Exam");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Exams)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Exam_Account");

            entity.HasOne(d => d.ExamMatrix).WithMany(p => p.Exams)
                .HasForeignKey(d => d.ExamMatrixId)
                .HasConstraintName("FK_Exam.ExamMatrixId");
        });

        modelBuilder.Entity<ExamExchange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExamExch__3214EC07CC24BCF5");

            entity.ToTable("ExamExchange");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Teacher).WithMany(p => p.ExamExchanges)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK_ExamExchange.TeacherId");
        });

        modelBuilder.Entity<ExamMatrix>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExamMatr__3214EC07FDDE59D3");

            entity.ToTable("ExamMatrix");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Grade).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Subject).WithMany(p => p.ExamMatrices)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK_ExamMatrix.SubjectId");
        });

        modelBuilder.Entity<ExamQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExamQues__3214EC0759C4D116");

            entity.ToTable("ExamQuestion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamQuestions)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("FK_ExamQuestion.ExamId");

            entity.HasOne(d => d.Question).WithMany(p => p.ExamQuestions)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_ExamQuestion.QuestionId");
        });

        modelBuilder.Entity<LikeComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LikeComm__3214EC0727B44068");

            entity.ToTable("LikeComment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.LikeComments)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_LikeComment.AccountId");

            entity.HasOne(d => d.Comment).WithMany(p => p.LikeComments)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK_LikeComment.CommentId");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC07D521D7BB");

            entity.ToTable("Location");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<MatrixSection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MatrixSe__3214EC07139DB572");

            entity.ToTable("MatrixSection");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.SectionName).HasMaxLength(200);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.ExamMatrix).WithMany(p => p.MatrixSections)
                .HasForeignKey(d => d.ExamMatrixId)
                .HasConstraintName("FK_MatrixSection.ExamMatrixId");
        });

        modelBuilder.Entity<MatrixSectionDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MatrixSe__3214EC07DDA01B53");

            entity.ToTable("MatrixSectionDetail");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Difficulty).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.BookChapter).WithMany(p => p.MatrixSectionDetails)
                .HasForeignKey(d => d.BookChapterId)
                .HasConstraintName("FK_MatrixSectionDetail.BookChapterId");

            entity.HasOne(d => d.BookTopic).WithMany(p => p.MatrixSectionDetails)
                .HasForeignKey(d => d.BookTopicId)
                .HasConstraintName("FK_MatrixSectionDetail.BookTopicId");

            entity.HasOne(d => d.MatrixSection).WithMany(p => p.MatrixSectionDetails)
                .HasForeignKey(d => d.MatrixSectionId)
                .HasConstraintName("FK_MatrixSectionDetail.MatrixSectionId");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07D803F53A");

            entity.ToTable("Question");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Level).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.BookTopic).WithMany(p => p.Questions)
                .HasForeignKey(d => d.BookTopicId)
                .HasConstraintName("FK_Question.BookTopicId");

            entity.HasOne(d => d.Category).WithMany(p => p.Questions)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Question.CategoryId");

            entity.HasOne(d => d.ExamExchange).WithMany(p => p.Questions)
                .HasForeignKey(d => d.ExamExchangeId)
                .HasConstraintName("FK_Question.ExamExchangeId");
        });

        modelBuilder.Entity<QuestionHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC070C0BB93C");

            entity.ToTable("QuestionHistory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Answer).HasMaxLength(50);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.YourAnswer).HasMaxLength(50);

            entity.HasOne(d => d.HistoryTest).WithMany(p => p.QuestionHistories)
                .HasForeignKey(d => d.HistoryTestId)
                .HasConstraintName("FK_QuestionHistory.HistoryTestId");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionHistories)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_QuestionHistory.QuestionId");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07C8687CCD");

            entity.ToTable("Quiz");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Quizzes)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Quiz_Account");

            entity.HasOne(d => d.BookChapter).WithMany(p => p.Quizzes)
                .HasForeignKey(d => d.BookChapterId)
                .HasConstraintName("FK_Quiz_BookChapter");

            entity.HasOne(d => d.BookTopic).WithMany(p => p.Quizzes)
                .HasForeignKey(d => d.BookTopicId)
                .HasConstraintName("FK_Quiz_BookTopic");
        });

        modelBuilder.Entity<QuizQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QuizQues__3214EC07A13964ED");

            entity.ToTable("QuizQuestion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Question).WithMany(p => p.QuizQuestions)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_QuizQuestion.QuestionId");

            entity.HasOne(d => d.Quiz).WithMany(p => p.QuizQuestions)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("FK_QuizQuestion.QuizId");
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reply__3214EC077FECEAA1");

            entity.ToTable("Reply");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Replies)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Reply.AccountId");

            entity.HasOne(d => d.Comment).WithMany(p => p.Replies)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK_Reply.CommentId");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Report__3214EC07DF60804E");

            entity.ToTable("Report");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.ReportedAccount).WithMany(p => p.ReportReportedAccounts)
                .HasForeignKey(d => d.ReportedAccountId)
                .HasConstraintName("FK_Report.ReportedAccountId");

            entity.HasOne(d => d.SendAccount).WithMany(p => p.ReportSendAccounts)
                .HasForeignKey(d => d.SendAccountId)
                .HasConstraintName("FK_Report.SendAccountId");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subject__3214EC07749F2CB4");

            entity.ToTable("Subject");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<SubjectBook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubjectB__3214EC07C26EE589");

            entity.ToTable("SubjectBook");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Subject).WithMany(p => p.SubjectBooks)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK_SubjectBook.SubjectId");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teacher__3214EC07A1BF5CE2");

            entity.ToTable("Teacher");

            entity.HasIndex(e => e.AccountId, "IX_Teacher_AccountId").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithOne(p => p.Teacher)
                .HasForeignKey<Teacher>(d => d.AccountId)
                .HasConstraintName("FK_Teacher_Account");

            entity.HasOne(d => d.Location).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK_Teacher.LocationId");
        });

        modelBuilder.Entity<TestHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestHist__3214EC07CB43148D");

            entity.ToTable("TestHistory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_TestHistory.AccountId");

            entity.HasOne(d => d.Exam).WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("FK_TestHistory_Exam");

            entity.HasOne(d => d.Quiz).WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("FK_TestHistory.QuizId");
        });

        modelBuilder.Entity<TestStorage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestStor__3214EC07B0E2ADC8");

            entity.ToTable("TestStorage");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.TestStorages)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_TestStorage.AccountId");

            entity.HasOne(d => d.Exam).WithMany(p => p.TestStorages)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("FK_TestStorage.ExamId");

            entity.HasOne(d => d.Quiz).WithMany(p => p.TestStorages)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("FK_TestStorage.QuizId");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC0743F611D8");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Deposit).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.DepositId)
                .HasConstraintName("FK_Transaction.DepositId");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK_Transaction.WalletId");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserInfo__3214EC07B7F19D0F");

            entity.ToTable("UserInfo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.UserInfos)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_UserInfo.AccountId");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC07435089C6");

            entity.ToTable("Wallet");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Wallet.AccountId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
