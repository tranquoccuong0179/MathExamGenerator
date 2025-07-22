using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.TestStorage
{
    public class CreateTestStorageRequest
    {
        public Guid? ExamId { get; set; }
        public bool? Liked { get; set; }
        public bool? Seen { get; set; }
    }
}
