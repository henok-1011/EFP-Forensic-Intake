using EFP_forensic.Models;

namespace EFP_forensic.Services.Interfaces;

public interface IFingerprintQualityService
{
    Task<FingerprintQualityResult> AssessAsync(FingerprintImage image, CancellationToken cancellationToken);
}
