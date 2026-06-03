using System.Text;
using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;

namespace EFP_forensic.Services.Mock;

public sealed class MockWsqEncodingService : IWsqEncodingService
{
    public async Task<WsqEncodingResult> EncodeAsync(FingerprintImage image, CancellationToken cancellationToken)
    {
        await Task.Delay(250, cancellationToken);

        var header = Encoding.ASCII.GetBytes($"FAKE-WSQ|{image.FingerPosition}|{image.CaptureMode}|{image.Width}x{image.Height}|");
        var sampleLength = Math.Min(4096, image.Pixels.Length);
        var wsqBytes = new byte[header.Length + sampleLength];
        Buffer.BlockCopy(header, 0, wsqBytes, 0, header.Length);

        for (var index = 0; index < sampleLength; index++)
        {
            wsqBytes[header.Length + index] = (byte)(image.Pixels[index] ^ 0x5A);
        }

        return new WsqEncodingResult
        {
            Success = true,
            WsqBytes = wsqBytes
        };
    }
}
