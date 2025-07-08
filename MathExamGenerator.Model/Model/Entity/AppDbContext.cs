using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MathExamGenerator.Model.Model.Entity;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=14.225.253.29,1433;Database=MathExamGenerator;User Id=sa;Password=winnertech123@;TrustServerCertificate=True;Encrypt=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC074096ED73");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsPremium).HasDefaultValue(false);
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Answer__3214EC07B5E20FD8");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Question).WithMany(p => p.Answers).HasConstraintName("FK_Answer.QuestionId");
        });

        modelBuilder.Entity<BookChapter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookChap__3214EC07A89A62ED");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.SubjectBook).WithMany(p => p.BookChapters).HasConstraintName("FK_BookChapter.SubjectBookId");
        });

        modelBuilder.Entity<BookTopic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookTopi__3214EC071C6DD74E");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.BookChapter).WithMany(p => p.BookTopics).HasConstraintName("FK_BookTopic.BookChapterId");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07976D58D8");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC0748C2919F");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.Comments).HasConstraintName("FK_Comment.AccountId");

            entity.HasOne(d => d.Question).WithMany(p => p.Comments).HasConstraintName("FK_Comment.QuestionId");
        });

        modelBuilder.Entity<Deposit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Deposit__3214EC07F4F27651");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.Deposits).HasConstraintName("FK_Deposit.AccountId");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exam__3214EC075E16BD22");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.Exams).HasConstraintName("FK_Exam_Account");

            entity.HasOne(d => d.ExamMatrix).WithMany(p => p.Exams).HasConstraintName("FK_Exam.ExamMatrixId");
        });

        modelBuilder.Entity<ExamExchange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExamExch__3214EC07CC24BCF5");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Teacher).WithMany(p => p.ExamExchanges).HasConstraintName("FK_ExamExchange.TeacherId");
        });

        modelBuilder.Entity<ExamMatrix>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC0753331762");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.ExamMatrices).HasConstraintName("FK_ExamMatrix_Account");

            entity.HasOne(d => d.Subject).WithMany(p => p.ExamMatrices).HasConstraintName("FK_ExamMatrix.SubjectId");
        });

        modelBuilder.Entity<ExamQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07184801BF");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamQuestions).HasConstraintName("FK_ExamQuestion.ExamId");

            entity.HasOne(d => d.Question).WithMany(p => p.ExamQuestions).HasConstraintName("FK_ExamQuestion.QuestionId");
        });

        modelBuilder.Entity<LikeComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LikeComm__3214EC0727B44068");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.LikeComments).HasConstraintName("FK_LikeComment.AccountId");

            entity.HasOne(d => d.Comment).WithMany(p => p.LikeComments).HasConstraintName("FK_LikeComment.CommentId");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC07D521D7BB");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<MatrixSection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MatrixSe__3214EC07139DB572");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ExamMatrix).WithMany(p => p.MatrixSections).HasConstraintName("FK_MatrixSection.ExamMatrixId");
        });

        modelBuilder.Entity<MatrixSectionDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MatrixSe__3214EC07DDA01B53");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.BookChapter).WithMany(p => p.MatrixSectionDetails).HasConstraintName("FK_MatrixSectionDetail.BookChapterId");

            entity.HasOne(d => d.BookTopic).WithMany(p => p.MatrixSectionDetails).HasConstraintName("FK_MatrixSectionDetail.BookTopicId");

            entity.HasOne(d => d.MatrixSection).WithMany(p => p.MatrixSectionDetails).HasConstraintName("FK_MatrixSectionDetail.MatrixSectionId");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07D803F53A");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.BookTopic).WithMany(p => p.Questions).HasConstraintName("FK_Question.BookTopicId");

            entity.HasOne(d => d.Category).WithMany(p => p.Questions).HasConstraintName("FK_Question.CategoryId");

            entity.HasOne(d => d.ExamExchange).WithMany(p => p.Questions).HasConstraintName("FK_Question.ExamExchangeId");
        });

        modelBuilder.Entity<QuestionHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC070C0BB93C");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.HistoryTest).WithMany(p => p.QuestionHistories).HasConstraintName("FK_QuestionHistory.HistoryTestId");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionHistories).HasConstraintName("FK_QuestionHistory.QuestionId");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07C8687CCD");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.Quizzes).HasConstraintName("FK_Quiz_Account");

            entity.HasOne(d => d.BookChapter).WithMany(p => p.Quizzes).HasConstraintName("FK_Quiz_BookChapter");

            entity.HasOne(d => d.BookTopic).WithMany(p => p.Quizzes).HasConstraintName("FK_Quiz_BookTopic");
        });

        modelBuilder.Entity<QuizQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QuizQues__3214EC07A13964ED");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Question).WithMany(p => p.QuizQuestions).HasConstraintName("FK_QuizQuestion.QuestionId");

            entity.HasOne(d => d.Quiz).WithMany(p => p.QuizQuestions).HasConstraintName("FK_QuizQuestion.QuizId");
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reply__3214EC077FECEAA1");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.Replies).HasConstraintName("FK_Reply.AccountId");

            entity.HasOne(d => d.Comment).WithMany(p => p.Replies).HasConstraintName("FK_Reply.CommentId");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Report__3214EC07DF60804E");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ReportedAccount).WithMany(p => p.ReportReportedAccounts).HasConstraintName("FK_Report.ReportedAccountId");

            entity.HasOne(d => d.SendAccount).WithMany(p => p.ReportSendAccounts).HasConstraintName("FK_Report.SendAccountId");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subject__3214EC07749F2CB4");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<SubjectBook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubjectB__3214EC07C26EE589");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Subject).WithMany(p => p.SubjectBooks).HasConstraintName("FK_SubjectBook.SubjectId");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teacher__3214EC07A1BF5CE2");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithOne(p => p.Teacher).HasConstraintName("FK_Teacher_Account");

            entity.HasOne(d => d.Location).WithMany(p => p.Teachers).HasConstraintName("FK_Teacher.LocationId");
        });

        modelBuilder.Entity<TestHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestHist__3214EC07CB43148D");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.TestHistories).HasConstraintName("FK_TestHistory.AccountId");

            entity.HasOne(d => d.Exam).WithMany(p => p.TestHistories).HasConstraintName("FK_TestHistory_Exam");

            entity.HasOne(d => d.Quiz).WithMany(p => p.TestHistories).HasConstraintName("FK_TestHistory.QuizId");
        });

        modelBuilder.Entity<TestStorage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestStor__3214EC07B0E2ADC8");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.TestStorages).HasConstraintName("FK_TestStorage.AccountId");

            entity.HasOne(d => d.Exam).WithMany(p => p.TestStorages).HasConstraintName("FK_TestStorage.ExamId");

            entity.HasOne(d => d.Quiz).WithMany(p => p.TestStorages).HasConstraintName("FK_TestStorage.QuizId");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC0743F611D8");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.Deposit).WithMany(p => p.Transactions).HasConstraintName("FK_Transaction.DepositId");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions).HasConstraintName("FK_Transaction.WalletId");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserInfo__3214EC07B7F19D0F");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.UserInfos).HasConstraintName("FK_UserInfo.AccountId");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallet__3214EC07435089C6");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.Wallets).HasConstraintName("FK_Wallet.AccountId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
