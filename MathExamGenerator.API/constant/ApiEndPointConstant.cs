﻿namespace MathExamGenerator.API.constant
{
    public static class ApiEndPointConstant
    {
        static ApiEndPointConstant()
        {
        }

        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;

        public static class Account
        {
            public const string AccountEndPoint = ApiEndpoint + "/account";
            public const string Otp = AccountEndPoint + "/otp";
            public const string Register = AccountEndPoint + "/register";
            public const string RegisterManager = AccountEndPoint + "/manager";
            public const string ChangePassword = AccountEndPoint + "/change-password";
            public const string ForgotPassword = AccountEndPoint + "/forgot-password";
            public const string VerifyOtp = AccountEndPoint + "/verify-otp";
            public const string ResetPassword = AccountEndPoint + "/reset-password";
            public const string ChangeAvatar = AccountEndPoint + "/avatar";
        }
        
        public static class GoogleAuthentication
        {
            public const string GoogleAuthEndPoint = ApiEndpoint + "/google-auth";
            public const string GoogleAuthLogin = GoogleAuthEndPoint + "/login";
            public const string GoogleAuthSignIn = GoogleAuthEndPoint + "/sign-in";
        }

        public static class Authentication
        {
            public const string AuthenticationEndPoint = ApiEndpoint + "/auth";
            public const string Authenticate = AuthenticationEndPoint;
        }

        public static class Teacher
        {
            public const string TeacherEndPoint = ApiEndpoint + "/teacher";
            public const string RegisterTeacher = TeacherEndPoint;
            public const string GetAllTeacher = TeacherEndPoint;
            public const string GetTeacher = TeacherEndPoint + "/{id}";
            public const string GetTeacherProfile = TeacherEndPoint + "/profile";
            public const string UpdateTeacher = TeacherEndPoint;
            public const string DeleteTeacher = TeacherEndPoint + "/{id}";
        }

        public static class BookTopic
        {
            public const string BookTopicEndPoint = ApiEndpoint + "/book-topic";
            public const string CreateBookTopic = BookTopicEndPoint;
            public const string GetAllBookTopic = BookTopicEndPoint;
            public const string GetBookTopic = BookTopicEndPoint + "/{id}";
            public const string UpdateBookTopic = BookTopicEndPoint + "/{id}";
            public const string DeleteBookTopic = BookTopicEndPoint + "/{id}";
            public const string GetAllQuestionByBookTopic = BookTopicEndPoint + "/{id}/questions";
        }

        public static class SubjectBook
        {
            public const string SubjectBookEndPoint = ApiEndpoint + "/subject-book";
            public const string CreateSubjectBook = SubjectBookEndPoint;
            public const string GetAllSubjectBooks = SubjectBookEndPoint;
            public const string GetSubjectBook = SubjectBookEndPoint + "/{id}";
            public const string GetAllChapterBySubjectBook = SubjectBookEndPoint + "/{id}/chapters";
            public const string UpdateSubjectBook = SubjectBookEndPoint + "/{id}";
            public const string DeleteSubjectBook = SubjectBookEndPoint + "/{id}";
        }

        public static class Location
        {
            public const string LocationEndPoint = ApiEndpoint + "/location";
            public const string GetAllLocations = LocationEndPoint;
        }

        public static class Exam
        {
            public const string ExamEndPoint = ApiEndpoint + "/exam";
            public const string GetAllExam = ExamEndPoint;
            public const string GetExamsOfCurrentUser = ExamEndPoint + "/my";
            public const string GetExam = ExamEndPoint + "/{id}";
            public const string GetAllQuestionByExam = ExamEndPoint + "/{id}/questions";
            public const string CreateExam = ExamEndPoint;
            public const string UpdateExam = ExamEndPoint + "/{id}";
            public const string DeleteExam = ExamEndPoint + "/{id}";
        }

        public static class ExamMatrix
        {
            public const string ExamMatrixEndPoint = ApiEndpoint + "/exam-matrix";
            public const string GetAllExamMatrix = ExamMatrixEndPoint;
            public const string GetExamMatrix = ExamMatrixEndPoint + "/{id}";
            public const string GetMatrixStructure = ExamMatrixEndPoint + "/{id}/structure";
            public const string CreateExamMatrix = ExamMatrixEndPoint;
            public const string UpdateExamMatrix = ExamMatrixEndPoint + "/{id}";
            public const string DeleteExamMatrix = ExamMatrixEndPoint + "/{id}";
            public const string GetSectionsByMatrixId = ExamMatrixEndPoint + "/{id}/sections";
        }

        public static class MatrixSection
        {
            public const string MatrixSectionEndPoint = ApiEndpoint + "/matrix-section";
            public const string GetMatrixSection = MatrixSectionEndPoint + "/{id}";
            public const string GetAllMatrixSection = MatrixSectionEndPoint;
            public const string UpdateMatrixSection = MatrixSectionEndPoint + "/{id}";
            public const string DeleteMatrixSection = MatrixSectionEndPoint + "/{id}";
            public const string GetAllDetailsBySectionId = MatrixSectionEndPoint + "/{id}/details";
        }

        public static class MatrixSectionDetail
        {
            public const string Endpoint = ApiEndpoint + "/matrix-section-detail";
            public const string GetAll = Endpoint;
            public const string GetById = Endpoint + "/{id}";
            public const string Update = Endpoint + "/{id}";
            public const string Delete = Endpoint + "/{id}";
        }

        public static class ExamEchange
        {
            public const string ExamExchangeEndPoint = ApiEndpoint + "/exam-exchange";
            public const string ExamExchangeTeacherEndPoint = ApiEndpoint + "/teacher";

            public const string GetExamExchange = ExamExchangeEndPoint;
            public const string CreateExamExchange = ExamExchangeEndPoint;
            public const string GetExamExchangeByTeacher = ExamExchangeTeacherEndPoint + "/exam-exchange";
            public const string GetExamExchangeById = ExamExchangeEndPoint + "/{id}";
            public const string UpdateExamExchange = ExamExchangeEndPoint + "/{id}";
            public const string DeleteExamExchange = ExamExchangeEndPoint + "/{id}";
            public const string GetAllTeacher = ExamExchangeEndPoint;
            public const string ApproveExamExchange = ExamExchangeEndPoint + "/status";
        }

        public static class User
        {
            public const string UserEndPoint = ApiEndpoint + "/user";
            public const string GetAllUsers = UserEndPoint;
            public const string GetUserProfile = UserEndPoint + "/profile";
            public const string GetUser = UserEndPoint + "/{id}";
            public const string UpdateUser = UserEndPoint;
            public const string DeleteUser = UserEndPoint + "/{id}";
        }

        public static class BookChapter
        {
            public const string BookChapterEndPoint = ApiEndpoint + "/chapter";
            public const string CreateBookChapter = BookChapterEndPoint;
            public const string GetAllBookChapters = BookChapterEndPoint;
            public const string GetBookChapter = BookChapterEndPoint + "/{id}";
            public const string GetAllBookTopicByChapter = BookChapterEndPoint + "/{id}/topics";
            public const string UpdateBookChapter = BookChapterEndPoint + "/{id}";
            public const string DeleteBookChapter = BookChapterEndPoint + "/{id}";
        }

        public static class Subject
        {
            public const string SubjectEndPoint = ApiEndpoint + "/subject";
            public const string CreateSubject = SubjectEndPoint;
            public const string GetAllSubjects = SubjectEndPoint;
            public const string GetSubject = SubjectEndPoint + "/{id}";
            public const string GetAllSubjectBookBySubject = SubjectEndPoint + "/{id}/subject-books";
            public const string UpdateSubject = SubjectEndPoint + "/{id}";
            public const string DeleteSubject = SubjectEndPoint + "/{id}";
        }

        public static class Payment
        {
            public const string PaymentEndPoint = ApiEndpoint + "/payment";
            public const string CreatePayment = PaymentEndPoint;
            public const string HandleWebhook = PaymentEndPoint + "/webhook";
        }
        public static class Comment
        {
            public const string CommentEndPoint = ApiEndpoint + "/comment";
            public const string CreateComment = CommentEndPoint;
            public const string UpdateComment = CommentEndPoint + "/{id}";
            public const string DeleteComment = CommentEndPoint + "/{id}";
            public const string LikeComment = CommentEndPoint + "/{id}/like";
            public const string ReplyComment = CommentEndPoint + "/{id}/reply";
            public const string GetAllReplyByComment = CommentEndPoint + "/{id}/reply";
        }

        public static class Question
        {
            public const string QuestionEndPoint = ApiEndpoint + "/question";
            public const string GetAllCommentByQuestion = QuestionEndPoint + "/{id}/comments";
            public const string GetAllQuestion = QuestionEndPoint;
            public const string DeleteQuestionById = QuestionEndPoint + "/{id}";
            public const string GetQuestionById = QuestionEndPoint + "/{id}";
            public const string GetQuestionSolution = QuestionEndPoint + "/solution";
        }

        public static class TestHistory
        {
            public const string TestHistoryEndpoint = ApiEndpoint + "/test-history";
            public const string GetAll = TestHistoryEndpoint;
            public const string GetById = TestHistoryEndpoint + "/{id}";
            public const string Create = TestHistoryEndpoint;
            public const string Update = TestHistoryEndpoint + "/{id}";
            public const string Delete = TestHistoryEndpoint + "/{id}";
            public const string GetQuestionHistoriesByTestId = TestHistoryEndpoint + "/{id}/question-histories";
        }

        public static class Reply
        {
            public const string ReplyEndPoint = ApiEndpoint + "/reply";
            public const string UpdateReply = ReplyEndPoint + "/{id}";
            public const string DeleteReply = ReplyEndPoint + "/{id}";
        }

        public static class QuestionHistory
        {
            public const string QuestionHistoryEndpoint = ApiEndpoint + "/question-history";
            public const string GetAll = QuestionHistoryEndpoint;
            public const string GetById = QuestionHistoryEndpoint + "/{id}";
            public const string Create = QuestionHistoryEndpoint;
            public const string Update = QuestionHistoryEndpoint + "/{id}";
            public const string Delete = QuestionHistoryEndpoint + "/{id}";
        }

        public static class TestStorage
        {
            public const string TestStorageEndpoint = ApiEndpoint + "/test-storage";
            public const string GetAll = TestStorageEndpoint;
            public const string GetById = TestStorageEndpoint + "/{id}";
            public const string Create = TestStorageEndpoint;
            public const string Update = TestStorageEndpoint + "/{id}";
            public const string Delete = TestStorageEndpoint + "/{id}";
        }

        public static class Quiz
        {
            public const string QuizEndPoint = ApiEndpoint + "/quiz";
            public const string CreateQuiz = QuizEndPoint;
            public const string GetAllQuiz = QuizEndPoint;
            public const string GetQuiz = QuizEndPoint + "/{id}";
        }

        public static class Transaction
        {
            public const string TransactionEndPoint = ApiEndpoint + "/transaction";
            public const string GetTransaction = TransactionEndPoint;
        }

        public static class Wallet
        {
            public const string WalletEndPoint = ApiEndpoint + "/wallet";
            public const string GetWalletByAccount = WalletEndPoint;
            
        }

        public static class Premium
        {
            public const string PremiumEndPoint = ApiEndpoint + "/premium";
            public const string BuyPremium = PremiumEndPoint + "/buy";
            public const string GetPremiumStatus = PremiumEndPoint + "/status";
        }
    }
}
