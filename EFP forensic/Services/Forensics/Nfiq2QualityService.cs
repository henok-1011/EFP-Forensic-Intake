using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;

namespace EFP_forensic.Services.Forensics;

public sealed class Nfiq2QualityService : IFingerprintQualityService
{
    public Task<FingerprintQualityResult> AssessAsync(FingerprintImage image, CancellationToken cancellationToken)
    {
        // TODO: Call nfiq2.dll through a vetted wrapper after native signatures and deployment are approved.
        throw new NotImplementedException("NFIQ2 native quality assessment is not integrated.");
    }
}
