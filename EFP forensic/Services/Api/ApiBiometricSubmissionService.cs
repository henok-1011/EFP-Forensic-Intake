using System.Net.Http;
using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;

namespace EFP_forensic.Services.Api;

public sealed class ApiBiometricSubmissionService : IBiometricSubmissionService
{
    private readonly HttpClient _httpClient;

    public ApiBiometricSubmissionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BiometricSubmissionResult> SubmitAsync(BiometricSubmissionPackage package, CancellationToken cancellationToken)
    {
        using var form = new MultipartFormDataContent
        {
            { new StringContent(package.TrackingNumber), "trackingNumber" },
            { new StringContent(package.ApplicantFullName), "applicantFullName" },
            { new StringContent(package.StationId), "stationId" },
            { new StringContent(package.OfficerId), "officerId" },
            { new StringContent(package.SubmittedAt.ToString("O")), "submittedAt" }
        };

        for (var index = 0; index < package.Fingerprints.Count; index++)
        {
            var fingerprint = package.Fingerprints[index];
            form.Add(new StringContent(fingerprint.FingerPosition.ToString()), $"fingerprints[{index}].fingerPosition");
            form.Add(new StringContent(fingerprint.CaptureMode.ToString()), $"fingerprints[{index}].captureMode");
            form.Add(new StringContent(fingerprint.QualityScore.ToString()), $"fingerprints[{index}].qualityScore");
            form.Add(new ByteArrayContent(fingerprint.WsqBytes), $"fingerprints[{index}].wsq", $"{fingerprint.FingerPosition}.wsq");
        }

        // Future hardening: add auth, idempotency keys, TLS pinning policy, and response validation.
        var response = await _httpClient.PostAsync("/api/v1/intake/submit-biometrics", form, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return new BiometricSubmissionResult
            {
                Success = false,
                Message = "Biometric submission service returned an error."
            };
        }

        return new BiometricSubmissionResult
        {
            Success = true,
            Message = "Biometrics Compiled Successfully. Record advanced to Forensic Review Queue.",
            ServerReference = response.Headers.TryGetValues("X-Server-Reference", out var values)
                ? values.FirstOrDefault() ?? string.Empty
                : string.Empty
        };
    }
}
