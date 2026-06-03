using EFP_forensic.Domain.Enums;
using EFP_forensic.Models;
using EFP_forensic.Services.Scanner;

namespace EFP_forensic.Services.Interfaces;

public interface IRealScanScannerDiagnostics
{
    bool IsSdkInitialized { get; }
    bool IsDeviceConnected { get; }
    RealScanDeviceSummary? ConnectedDevice { get; }
    Task<RealScanDiagnosticResult> InitializeSdkAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<RealScanDeviceSummary>> EnumerateDevicesAsync(CancellationToken cancellationToken);
    Task ConnectAsync(int deviceListIndex, CancellationToken cancellationToken);
    Task<RealScanDeviceInfo?> GetConnectedDeviceInfoAsync(CancellationToken cancellationToken);
    Task<RealScanQualityModeResult> SetQualityModeAsync(int qualityMode, CancellationToken cancellationToken);
    Task<SlapCaptureResult> CaptureSlapGroupAsync(SlapCaptureGroup group, CancellationToken cancellationToken);
}
