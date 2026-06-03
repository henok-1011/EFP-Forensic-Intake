using EFP_forensic.Domain.Enums;

namespace EFP_forensic.Models;

public sealed class SlapCaptureResult
{
    public bool Success { get; init; }
    public SlapCaptureGroup Group { get; init; }
    public IReadOnlyList<SegmentedFingerprintImage> Segments { get; init; } = [];
    public int SdkResult { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
}

public sealed class SegmentedFingerprintImage
{
    public FingerPosition FingerPosition { get; init; }
    public FingerprintImage Image { get; init; } = new();
}
