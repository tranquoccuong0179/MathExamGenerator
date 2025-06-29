﻿using MathExamGenerator.Model.Payload.Request.QuestionHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.TestHistory
{
    public class UpdateTestHistoryRequest
    {
        public Guid? ExamId { get; set; }
        public Guid? QuizId { get; set; }
        public double? Grade { get; set; }
        public string? Status { get; set; }
        public TimeOnly? StartAt { get; set; }
        public List<UpdateQuestionHistoryRequest> QuestionHistories { get; set; }
    }
}
