﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.Subject
{
    public class CreateSubjectRequest
    {
        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;
    }
}
