﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.Answer
{
    public class AnswerReQuest
    {
        public string Content { get; set; } = default!;
        public string? Image { get; set; }
        public bool IsTrue { get; set; }
    }

}
