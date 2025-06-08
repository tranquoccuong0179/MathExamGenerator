using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Request.Authentication
{
    public class AuthenticateRequest
    {
        public string UsernameOrEmailOrPhone { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
