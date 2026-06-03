using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;

namespace EFP_forensic.Services.Mock;

public sealed class MockFingerprintQualityService : IFingerprintQualityService
{
    private readonly Random _random = new();

    public async Task<FingerprintQualityResult> AssessAsync(FingerprintImage image, CancellationToken cancellationToken)
    {
        await Task.Delay(300, cancellationToken);
        var score = _random.Next(45, 96);

        return new FingerprintQualityResult
        {
            Score = score,
            Passed = score >= 60,
            Message = score switch
            {
                < 60 => "Poor Ridge Definition",
                < 80 => "Acceptable Quality",
                _ => "Excellent Quality"
            }
        };
    }
}
