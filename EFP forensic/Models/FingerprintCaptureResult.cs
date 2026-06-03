namespace EFP_forensic.Models;

public sealed class FingerprintCaptureResult
{
    public bool Success { get; init; }
    public FingerprintImage? Image { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
}
