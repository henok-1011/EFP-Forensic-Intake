using System.Runtime.CompilerServices;
using EFP_forensic.Domain.Enums;
using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;

namespace EFP_forensic.Services.Mock;

public sealed class MockFingerprintScanner : IFingerprintScanner
{
    private const int Width = 360;
    private const int Height = 420;
    private readonly Random _random = new();
    private bool _connected;

    public async Task ConnectAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(350, cancellationToken);
        _connected = true;
    }

    public Task DisconnectAsync()
    {
        _connected = false;
        return Task.CompletedTask;
    }

    public async IAsyncEnumerable<FingerprintFrame> StreamPreviewAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (!_connected)
        {
            await ConnectAsync(cancellationToken);
        }

        while (!cancellationToken.IsCancellationRequested)
        {
            yield return new FingerprintFrame
            {
                Pixels = GenerateFingerprintPixels(Width, Height),
                Width = Width,
                Height = Height,
                Dpi = 500,
                Timestamp = DateTimeOffset.UtcNow
            };

            await Task.Delay(55, cancellationToken);
        }
    }

    public async Task<FingerprintCaptureResult> CaptureAsync(FingerPosition finger, CaptureMode mode, CancellationToken cancellationToken)
    {
        if (!_connected)
        {
            await ConnectAsync(cancellationToken);
        }

        await Task.Delay(500, cancellationToken);

        return new FingerprintCaptureResult
        {
            Success = true,
            Image = new FingerprintImage
            {
                Pixels = GenerateFingerprintPixels(Width, Height),
                Width = Width,
                Height = Height,
                Dpi = 500,
                FingerPosition = finger,
                CaptureMode = mode,
                CapturedAt = DateTimeOffset.UtcNow
            }
        };
    }

    private byte[] GenerateFingerprintPixels(int width, int height)
    {
        var pixels = new byte[width * height];
        var centerX = width / 2.0;
        var centerY = height / 2.0;
        var phase = _random.NextDouble() * Math.PI;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var dx = x - centerX;
                var dy = y - centerY;
                var angle = Math.Atan2(dy, dx);
                var distance = Math.Sqrt(dx * dx + dy * dy);
                var ridge = Math.Sin(distance * 0.16 + Math.Sin(angle * 6) * 4 + phase);
                var noise = _random.Next(-18, 19);
                var value = 142 + (int)(ridge * 68) + noise;
                pixels[y * width + x] = (byte)Math.Clamp(value, 20, 245);
            }
        }

        return pixels;
    }
}
