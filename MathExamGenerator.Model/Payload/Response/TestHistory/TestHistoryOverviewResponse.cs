﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.TestHistory
{
    public class TestHistoryOverviewResponse
    {
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? ExamId { get; set; }
        public Guid? QuizId { get; set; }
        public string? Name { get; set; }
        public double? Grade { get; set; }
        public string? Status { get; set; }
        public string? TotalQuestion { get; set; }
        public TimeSpan? StartAt { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
