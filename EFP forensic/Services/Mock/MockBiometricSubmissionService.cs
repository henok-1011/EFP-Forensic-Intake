using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;

namespace EFP_forensic.Services.Mock;

public sealed class MockBiometricSubmissionService : IBiometricSubmissionService
{
    public async Task<BiometricSubmissionResult> SubmitAsync(BiometricSubmissionPackage package, CancellationToken cancellationToken)
    {
        await Task.Delay(900, cancellationToken);

        if (package.Fingerprints.Count == 0 && package.MissingFingerprints.Count == 0)
        {
            return new BiometricSubmissionResult
            {
                Success = false,
                Message = "No accepted fingerprints or missing finger records are available for submission."
            };
        }

        return new BiometricSubmissionResult
        {
            Success = true,
            Message = "Biometrics Compiled Successfully. Record advanced to Forensic Review Queue.",
            ServerReference = $"FRQ-{DateTimeOffset.UtcNow:yyyyMMddHHmmss}"
        };
    }
}
