using EFP_forensic.Domain.Enums;
using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;

namespace EFP_forensic.Services.Mock;

public sealed class MockIntakeVerificationService : IIntakeVerificationService
{
    public async Task<IntakeVerificationResult> VerifyAsync(string trackingNumber, CancellationToken cancellationToken)
    {
        await Task.Delay(700, cancellationToken);

        if (string.IsNullOrWhiteSpace(trackingNumber))
        {
            return new IntakeVerificationResult
            {
                Success = false,
                ErrorType = VerificationErrorType.Unknown,
                Message = "Enter a tracking number before verification."
            };
        }

        var normalized = trackingNumber.Trim().ToUpperInvariant();
        if (normalized.Contains("404", StringComparison.Ordinal))
        {
            return new IntakeVerificationResult
            {
                Success = false,
                ErrorType = VerificationErrorType.NotFound,
                Message = "Tracking Number Not Found"
            };
        }

        if (normalized.Contains("UNPAID", StringComparison.Ordinal))
        {
            return new IntakeVerificationResult
            {
                Success = false,
                ErrorType = VerificationErrorType.PaymentPending,
                Message = "Payment Verification Pending. Cannot capture biometrics."
            };
        }

        if (normalized.Contains("DONE", StringComparison.Ordinal))
        {
            return new IntakeVerificationResult
            {
                Success = false,
                ErrorType = VerificationErrorType.AlreadyProcessed,
                Message = "Record already processed. Duplicate intake denied."
            };
        }

        return new IntakeVerificationResult
        {
            Success = true,
            Message = "Applicant ready for biometric intake.",
            ApplicantProfile = new ApplicantProfile
            {
                TrackingNumber = trackingNumber.Trim(),
                FullName = "Abebe Tesfaye",
                DateOfBirth = new DateOnly(1992, 5, 18),
                NationalId = "ETH-1234567890",
                PassportNumber = "EP1234567",
                PaymentStatus = PaymentStatus.Paid,
                IntakeStatus = IntakeStatus.PendingIntake,
                DocumentUrls =
                [
                    "/uploads/documents/national-id-front.jpg",
                    "/uploads/documents/passport-scan.pdf"
                ]
            }
        };
    }
}
