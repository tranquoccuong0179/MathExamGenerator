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
        }

        public static class SubjectBook
        {
            public const string SubjectBookEndPoint = ApiEndpoint + "/subject-book";
            public const string GetAllChapterBySubjectBook = SubjectBookEndPoint + "/{id}/chapters";
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
            public const string GetExam = ExamEndPoint + "/{id}";
            public const string GetAllQuestionByExam = ExamEndPoint + "/{id}/questions";
            public const string CreateExam = ExamEndPoint;
            public const string UpdateExam = ExamEndPoint + "/{id}";
            public const string DeleteExam = ExamEndPoint + "/{id}";
        }
    }
}
