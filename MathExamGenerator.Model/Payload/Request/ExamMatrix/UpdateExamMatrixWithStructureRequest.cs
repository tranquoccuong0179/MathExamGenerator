﻿using MathExamGenerator.Model.Payload.Request.MatrixSection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.ExamMatrix
{
    public class UpdateExamMatrixWithStructureRequest
    {
        public string? Name { get; set; }
        public string? Grade { get; set; }
        public string? Description { get; set; }
        public Guid SubjectId { get; set; }

        public List<UpdateMatrixSectionWithDetailRequest> Sections { get; set; } = new();
    }
}
