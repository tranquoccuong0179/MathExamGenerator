using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.Package
{
    public class CreatePackageRequest
    {
        public string Name { get; set; } = default!;

        public decimal Price { get; set; }

        public int Point { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
