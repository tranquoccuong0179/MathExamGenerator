using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Chapter
{
    public class GetChapterResponse
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public int? ChapterNo { get; set; }

        public Guid? SubjectBookId { get; set; }
    }
}
