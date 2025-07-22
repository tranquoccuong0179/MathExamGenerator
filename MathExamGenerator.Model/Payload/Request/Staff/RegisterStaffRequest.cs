using MathExamGenerator.Model.Enum;
using Microsoft.AspNetCore.Http;

namespace MathExamGenerator.Model.Payload.Request.Staff;

public class RegisterStaffRequest
{
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
    
    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public GenderEnum Gender { get; set; }

    public IFormFile? AvatarUrl { get; set; }
}