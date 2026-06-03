using EFP_forensic.Domain.Enums;

namespace EFP_forensic.Models;

public sealed class FingerprintImage
{
    public byte[] Pixels { get; init; } = [];
    public int Width { get; init; }
    public int Height { get; init; }
    public int Dpi { get; init; }
    public string Source { get; init; } = string.Empty;
    public int CaptureSdkResult { get; init; }
    public FingerPosition FingerPosition { get; init; }
    public CaptureMode CaptureMode { get; init; }
    public DateTimeOffset CapturedAt { get; init; } = DateTimeOffset.UtcNow;
}
