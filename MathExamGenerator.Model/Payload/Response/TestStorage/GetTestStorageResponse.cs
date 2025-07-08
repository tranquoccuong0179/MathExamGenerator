using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.TestStorage
{
    public class GetTestStorageResponse
    {
        public Guid Id { get; set; }
        public Guid? ExamId { get; set; }
        public Guid? QuizId { get; set; }
        public string? Name { get; set; }
        public bool? Liked { get; set; }
        public bool? Seen { get; set; }
        public DateTime? CreateAt { get; set; }
    }

}
