using EFP_forensic.Models;

namespace EFP_forensic.Services.Interfaces;

public interface ISecureBufferCleanupService
{
    void ClearFingerprintImage(FingerprintImage image);
    void ClearSubmissionPackage(BiometricSubmissionPackage package);
}
