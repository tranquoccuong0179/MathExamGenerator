using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathExamGenerator.Model.Enum;
using Microsoft.AspNetCore.Http;

namespace MathExamGenerator.Model.Payload.Request.Manager
{
    public class RegisterManagerRequest
    {
        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

        public GenderEnum Gender { get; set; }

        public IFormFile? AvatarUrl { get; set; }

    }
}
