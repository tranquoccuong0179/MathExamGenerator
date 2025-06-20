using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.User
{
    public class GetUserResponse
    {
        public Guid? AccountId { get; set; }

        public Guid UserId { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public int? QuizFree { get; set; }

        public double? Point { get; set; }
    }
}
