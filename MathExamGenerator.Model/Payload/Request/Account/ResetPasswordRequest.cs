﻿namespace MathExamGenerator.Model.Payload.Request.Account;

public class ResetPasswordRequest
{
    public string Email { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}