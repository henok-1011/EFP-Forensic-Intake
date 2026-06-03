using EFP_forensic.Models;

namespace EFP_forensic.Services.Interfaces;

public interface IBiometricSubmissionService
{
    Task<BiometricSubmissionResult> SubmitAsync(BiometricSubmissionPackage package, CancellationToken cancellationToken);
}
