namespace EFP_forensic.Models;

public sealed class BiometricSubmissionPackage
{
    public string TrackingNumber { get; init; } = string.Empty;
    public string ApplicantFullName { get; init; } = string.Empty;
    public List<CapturedFingerprint> Fingerprints { get; init; } = [];
    public List<MissingFingerprint> MissingFingerprints { get; init; } = [];
    public string StationId { get; init; } = string.Empty;
    public string OfficerId { get; init; } = string.Empty;
    public string ScannerModel { get; init; } = string.Empty;
    public string ScannerSerial { get; init; } = string.Empty;
    public DateTimeOffset SubmittedAt { get; init; } = DateTimeOffset.UtcNow;
}
