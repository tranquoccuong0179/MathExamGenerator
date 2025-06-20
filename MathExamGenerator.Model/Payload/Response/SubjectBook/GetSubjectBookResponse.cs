using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.SubjectBook
{
    public class GetSubjectBookResponse
    {
        public Guid Id { get; set; }

        public Guid? SubjectId { get; set; }

        public string? Title { get; set; }

        public string? FileUrl { get; set; }

        public string? Description { get; set; }
    }
}
