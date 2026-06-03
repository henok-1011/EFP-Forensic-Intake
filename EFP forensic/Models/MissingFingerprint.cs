using EFP_forensic.Domain.Enums;

namespace EFP_forensic.Models;

public sealed class MissingFingerprint
{
    public FingerPosition FingerPosition { get; init; }
    public bool IsMissing { get; init; } = true;
    public string MissingReason { get; init; } = string.Empty;
    public DateTimeOffset MarkedMissingAt { get; init; }
}
