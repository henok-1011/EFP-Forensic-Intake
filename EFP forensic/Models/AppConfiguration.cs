namespace EFP_forensic.Models;

public sealed class AppConfiguration
{
    public string ApiBaseUrl { get; init; } = "https://localhost:5001";
    public string StationId { get; init; } = "EFP-WS-001";
    public string OfficerId { get; init; } = "Intake Officer";
    public bool UseMockScanner { get; init; } = true;
    public bool UseMockApi { get; init; } = true;
    public int QualityThreshold { get; init; } = 60;
}
