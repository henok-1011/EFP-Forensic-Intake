using EFP_forensic.Domain.Enums;

namespace EFP_forensic.Models;

public sealed class IntakeVerificationResult
{
    public bool Success { get; init; }
    public ApplicantProfile? ApplicantProfile { get; init; }
    public VerificationErrorType ErrorType { get; init; } = VerificationErrorType.None;
    public string Message { get; init; } = string.Empty;
}
