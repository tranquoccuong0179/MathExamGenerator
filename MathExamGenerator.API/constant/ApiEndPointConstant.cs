namespace MathExamGenerator.API.constant
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
            public const string UpdateTeacher = TeacherEndPoint;
            public const string DeleteTeacher = TeacherEndPoint + "/{id}";
        }

        public static class BookTopic
        {
            public const string BookTopicEndPoint = ApiEndpoint + "/book-topic";
            public const string GetAllBookTopic = BookTopicEndPoint;
            public const string GetBookTopic = BookTopicEndPoint + "/{id}";
        }

        public static class SubjectBook
        {
            public const string SubjectBookEndPoint = ApiEndpoint + "/subject-book";
            public const string GetAllSubjectBooks = SubjectBookEndPoint;
            public const string GetSubjectBook = SubjectBookEndPoint + "/{id}";
            public const string GetAllChapterBySubjectBook = SubjectBookEndPoint + "/{id}/chapters";
        }

        public static class Location
        {
            public const string LocationEndPoint = ApiEndpoint + "/location";
            public const string GetAllLocations = LocationEndPoint;
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


        public static class User
        {
            public const string UserEndPoint = ApiEndpoint + "/user";
            public const string GetAllUsers = UserEndPoint;
            public const string GetUser = UserEndPoint + "/{id}";
            public const string UpdateUser = UserEndPoint;
            public const string DeleteUser = UserEndPoint + "/{id}";
        }

        public static class BookChapter
        {
            public const string BookChapterEndPoint = ApiEndpoint + "/chapter";
            public const string GetAllBookChapters = BookChapterEndPoint;
            public const string GetBookChapter = BookChapterEndPoint + "/{id}";
            public const string GetAllBookTopicByChapter = BookChapterEndPoint + "/{id}/topics";
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
    }
}
