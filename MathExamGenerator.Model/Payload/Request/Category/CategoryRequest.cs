using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.Category
{
    public class CategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

    }
}
