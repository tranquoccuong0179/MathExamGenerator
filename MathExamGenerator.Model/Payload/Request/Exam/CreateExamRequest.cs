using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.Exam
{
    public class CreateExamRequest
    {
        public string Name { get; set; }
        public long Time { get; set; }
        public int? Quantity { get; set; }
        public DateTime? StartDate { get; set; }    
        public DateTime? EndDate { get; set; }       
        public bool? Minigame { get; set; } = false;
        public Guid? ExamMatrixId { get; set; }
    }
}
