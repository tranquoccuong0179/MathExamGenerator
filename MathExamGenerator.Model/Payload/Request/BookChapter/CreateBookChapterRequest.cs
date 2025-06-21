using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.BookChapter
{
    public class CreateBookChapterRequest
    {
        public string Name { get; set; } = null!;

        public int ChapterNo { get; set; }

        public Guid SubjectBookId { get; set; }

    }
}
