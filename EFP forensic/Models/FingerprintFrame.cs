namespace EFP_forensic.Models;

public sealed class FingerprintFrame
{
    public byte[] Pixels { get; init; } = [];
    public int Width { get; init; }
    public int Height { get; init; }
    public int Dpi { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}
