using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.SubjectBook
{
    public class CreateSubjectBookResponse
    {
        public string? Title { get; set; }

        public string? FileUrl { get; set; }
        
        public string? BookImage { get; set; }

        public string? Description { get; set; }

        public Guid? SubjectId { get; set; }

    }
}
