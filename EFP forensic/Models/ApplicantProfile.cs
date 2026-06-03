using EFP_forensic.Domain.Enums;

namespace EFP_forensic.Models;

public sealed class ApplicantProfile
{
    public string TrackingNumber { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public DateOnly DateOfBirth { get; init; }
    public string NationalId { get; init; } = string.Empty;
    public string PassportNumber { get; init; } = string.Empty;
    public PaymentStatus PaymentStatus { get; init; } = PaymentStatus.Unknown;
    public IntakeStatus IntakeStatus { get; init; } = IntakeStatus.Unknown;
    public IReadOnlyList<string> DocumentUrls { get; init; } = Array.Empty<string>();
}
