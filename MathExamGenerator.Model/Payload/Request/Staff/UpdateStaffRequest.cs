using MathExamGenerator.Model.Enum;

namespace MathExamGenerator.Model.Payload.Request.Staff;

public class UpdateStaffRequest
{
    public string? FullName { get; set; }
    
    public string? Phone { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public GenderEnum? Gender { get; set; }
}