using System.Net.Http;
using System.Net.Http.Json;
using EFP_forensic.Domain.Enums;
using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;

namespace EFP_forensic.Services.Api;

public sealed class ApiIntakeVerificationService : IIntakeVerificationService
{
    private readonly HttpClient _httpClient;

    public ApiIntakeVerificationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IntakeVerificationResult> VerifyAsync(string trackingNumber, CancellationToken cancellationToken)
    {
        try
        {
            var escaped = Uri.EscapeDataString(trackingNumber);
            var response = await _httpClient.GetAsync($"/api/v1/intake/verify/{escaped}", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return new IntakeVerificationResult
                {
                    Success = false,
                    ErrorType = response.StatusCode == System.Net.HttpStatusCode.NotFound
                        ? VerificationErrorType.NotFound
                        : VerificationErrorType.ServerError,
                    Message = "Verification service returned an error."
                };
            }

            var result = await response.Content.ReadFromJsonAsync<IntakeVerificationResult>(cancellationToken);
            return result ?? new IntakeVerificationResult
            {
                Success = false,
                ErrorType = VerificationErrorType.ServerError,
                Message = "Verification service returned an empty response."
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (HttpRequestException)
        {
            return new IntakeVerificationResult
            {
                Success = false,
                ErrorType = VerificationErrorType.NetworkError,
                Message = "Network error while verifying the tracking number."
            };
        }
    }
}
