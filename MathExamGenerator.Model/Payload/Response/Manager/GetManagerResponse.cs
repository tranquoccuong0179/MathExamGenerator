namespace MathExamGenerator.Model.Payload.Response.Manager;

public class GetManagerResponse
{
    public Guid Id { get; set; }

    public string? UserName { get; set; }
    
    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? AvatarUrl { get; set; }
}