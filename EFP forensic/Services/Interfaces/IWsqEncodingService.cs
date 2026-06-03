using EFP_forensic.Models;

namespace EFP_forensic.Services.Interfaces;

public interface IWsqEncodingService
{
    Task<WsqEncodingResult> EncodeAsync(FingerprintImage image, CancellationToken cancellationToken);
}
