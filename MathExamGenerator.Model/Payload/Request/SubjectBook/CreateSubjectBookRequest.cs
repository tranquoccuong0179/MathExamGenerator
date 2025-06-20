using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.SubjectBook
{
    public class CreateSubjectBookRequest
    {
        public string Title { get; set; } = null!;

        public string FileUrl { get; set; } = null!;

        public string Description { get; set; } = null!;

        public Guid SubjectId { get; set; }

    }
}
