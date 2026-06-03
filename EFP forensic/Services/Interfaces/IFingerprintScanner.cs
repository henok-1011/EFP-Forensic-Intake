using EFP_forensic.Domain.Enums;
using EFP_forensic.Models;

namespace EFP_forensic.Services.Interfaces;

public interface IFingerprintScanner
{
    Task ConnectAsync(CancellationToken cancellationToken);
    Task DisconnectAsync();
    IAsyncEnumerable<FingerprintFrame> StreamPreviewAsync(CancellationToken cancellationToken);
    Task<FingerprintCaptureResult> CaptureAsync(FingerPosition finger, CaptureMode mode, CancellationToken cancellationToken);
}
