﻿using MathExamGenerator.Model.Payload.Response.QuestionHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.ExamDoing
{
    public class GetExamDoingResponse
    {
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? ExamId { get; set; }
        public string? Name { get; set; }
        public double? Grade { get; set; }
        public string? Status { get; set; }
        public string? TotalQuestion { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public List<GetQuestionHistoryResponse> QuestionHistories { get; set; }
    }
}
