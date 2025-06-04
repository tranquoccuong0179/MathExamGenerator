using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Exceptions
{
    public class ForbiddentException : Exception
    {
        public ForbiddentException(string message) : base(message)
        {
        }
    }
}
