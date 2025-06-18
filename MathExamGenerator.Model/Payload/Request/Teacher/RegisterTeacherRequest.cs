using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Enum;

namespace MathExamGenerator.Model.Payload.Request.Teacher
{
    public class RegisterTeacherRequest
    {
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Otp { get; set; } = null!;

        public DateOnly? DateOfBirth { get; set; }

        public GenderEnum Gender { get; set; }

        public string? AvatarUrl { get; set; }

        public string? Description { get; set; }

        public string SchoolName { get; set; } = null!;

        public Guid LocationId { get; set; }
    }
}
