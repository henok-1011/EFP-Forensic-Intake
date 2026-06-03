using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;

namespace EFP_forensic.Services.Security;

public sealed class SecureBufferCleanupService : ISecureBufferCleanupService
{
    public void ClearFingerprintImage(FingerprintImage image)
    {
        if (image.Pixels.Length > 0)
        {
            Array.Clear(image.Pixels);
        }
    }

    public void ClearSubmissionPackage(BiometricSubmissionPackage package)
    {
        foreach (var fingerprint in package.Fingerprints)
        {
            if (fingerprint.WsqBytes.Length > 0)
            {
                Array.Clear(fingerprint.WsqBytes);
            }
        }

        package.Fingerprints.Clear();
        package.MissingFingerprints.Clear();
    }
}
