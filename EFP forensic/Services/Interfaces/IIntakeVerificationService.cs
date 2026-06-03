using EFP_forensic.Models;

namespace EFP_forensic.Services.Interfaces;

public interface IIntakeVerificationService
{
    Task<IntakeVerificationResult> VerifyAsync(string trackingNumber, CancellationToken cancellationToken);
}
