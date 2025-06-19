using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.Teacher
{
    public class UpdateTeacherRequest
    {
        public string? Description { get; set; }

        public string? SchoolName { get; set; }

        public Guid? LocationId { get; set; }
    }
}
