﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Exam
{
    public class GetExamResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long Time { get; set; }
        public string Status { get; set; }
        public Guid ExamMatrixId { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
