using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Teacher
{
    public class GetTeacherResponse
    {
        public Guid AccountId { get; set; }

        public Guid TeacherId { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Description { get; set; }
        
        public string? AvatarUrl { get; set; }

        public string? SchoolName { get; set; }

        public string? LocationName { get; set; }
    }
}
