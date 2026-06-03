namespace EFP_forensic.Models;

public sealed class FingerprintQualityResult
{
    public int Score { get; init; }
    public bool Passed { get; init; }
    public string Message { get; init; } = string.Empty;
}
