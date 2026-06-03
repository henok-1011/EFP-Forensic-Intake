namespace EFP_forensic.Models;

public sealed class WsqEncodingResult
{
    public bool Success { get; init; }
    public byte[] WsqBytes { get; init; } = [];
    public string ErrorMessage { get; init; } = string.Empty;
}
