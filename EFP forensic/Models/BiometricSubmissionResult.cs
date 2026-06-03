namespace EFP_forensic.Models;

public sealed class BiometricSubmissionResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public string ServerReference { get; init; } = string.Empty;
}
