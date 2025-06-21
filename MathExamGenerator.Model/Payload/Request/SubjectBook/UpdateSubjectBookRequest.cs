using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.SubjectBook
{
    public class UpdateSubjectBookRequest
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public Guid? SubjectId { get; set; }
    }
}
