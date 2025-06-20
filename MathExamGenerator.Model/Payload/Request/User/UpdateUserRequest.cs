using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Enum;

namespace MathExamGenerator.Model.Payload.Request.User
{
    public class UpdateUserRequest
    {
        public string? FullName { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public GenderEnum? Gender { get; set; }
    }
}
