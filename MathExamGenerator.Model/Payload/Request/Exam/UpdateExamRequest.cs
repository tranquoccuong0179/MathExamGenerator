﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.Exam
{
    public class UpdateExamRequest
    {
        public string? Name { get; set; }
        public long? Time { get; set; }
    }
}
