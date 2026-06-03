using EFP_forensic.Domain.Enums;

namespace EFP_forensic.Models;

public sealed class CapturedFingerprint
{
    public FingerPosition FingerPosition { get; init; }
    public CaptureMode CaptureMode { get; init; }
    public CaptureSource CaptureSource { get; init; }
    public SlapCaptureGroup ParentSlapGroup { get; init; } = SlapCaptureGroup.None;
    public int QualityScore { get; init; }
    public string QualityLabel { get; init; } = string.Empty;
    public byte[] WsqBytes { get; init; } = [];
    public int Width { get; init; }
    public int Height { get; init; }
    public int Dpi { get; init; }
    public double AveragePixelValue { get; init; }
    public DateTimeOffset CapturedAt { get; init; }
}
