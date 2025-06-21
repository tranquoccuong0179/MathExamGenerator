using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MathExamGenerator.Model.Payload.Request.SubjectBook
{
    public class CreateSubjectBookRequest
    {
        public string Title { get; set; } = null!;

        public IFormFile FileUrl { get; set; } = null!;

        public string Description { get; set; } = null!;

        public Guid SubjectId { get; set; }

    }
}
