using System.Diagnostics;
using System.Runtime.InteropServices;
using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;
using EFP_forensic.Services.Scanner;
using RealScan;

namespace EFP_forensic.Services.Forensics;

public sealed class RealScanQualityService : IFingerprintQualityService
{
    private const int PreferredQualityMode = 2;

    private readonly IRealScanScannerDiagnostics? _scannerDiagnostics;

    public RealScanQualityService(IRealScanScannerDiagnostics? scannerDiagnostics = null)
    {
        _scannerDiagnostics = scannerDiagnostics;
    }

    public async Task<FingerprintQualityResult> AssessAsync(FingerprintImage image, CancellationToken cancellationToken)
    {
        var diagnostic = await ScoreCapturedImageAsync(image, GetImageSource(image, "Captured image"), cancellationToken);
        if (diagnostic.SdkResult != RealScanSDK.RS_SUCCESS)
        {
            return new FingerprintQualityResult
            {
                Score = 0,
                Passed = false,
                Message = diagnostic.Message
            };
        }

        return CreateQualityResult(diagnostic.QualityScore);
    }

    public async Task<RealScanQualityDiagnosticResult> ScoreCapturedImageAsync(
        FingerprintImage image,
        string source,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var qualityMode = await SetQualityModeForScoringAsync(cancellationToken);
        var scoreSource = string.IsNullOrWhiteSpace(source)
            ? GetImageSource(image, "Captured image")
            : source;
        var notes =
            "Shared diagnostics/main capture scoring path. Raw 8-bit grayscale buffer only; no WPF conversion, stride, BGRA/RGBA, or preview frame. " +
            "RS_GetQualityScore called as (imagePtr, width, height). " +
            $"RS_SetParam(deviceHandle, RS_GVAR_FP_QUALITY_MODE, ref iq), iq={PreferredQualityMode}, result={qualityMode.SdkResult}, message={qualityMode.Message}.";

        return await Task.Run(
            () => RunVariant("SDK final captured buffer", scoreSource, image.Pixels, image.Width, image.Height, notes),
            cancellationToken);
    }

    public Task<IReadOnlyList<RealScanQualityDiagnosticResult>> RunQualityDiagnosticsAsync(
        FingerprintImage image,
        CancellationToken cancellationToken)
    {
        return Task.Run<IReadOnlyList<RealScanQualityDiagnosticResult>>(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            var results = new List<RealScanQualityDiagnosticResult>
            {
                RunVariant("Original captured 8bpp buffer", image.Pixels, image.Width, image.Height)
            };

            cancellationToken.ThrowIfCancellationRequested();

            var inverted = Invert(image.Pixels);
            results.Add(RunVariant("Inverted captured buffer", inverted, image.Width, image.Height));

            cancellationToken.ThrowIfCancellationRequested();

            var cropped = CropFingerprintBoundingBox(image.Pixels, image.Width, image.Height);
            results.Add(RunVariant("Cropped fingerprint bounding box buffer", cropped.Pixels, cropped.Width, cropped.Height));

            return results;
        }, cancellationToken);
    }

    public RealScanQualityDiagnosticResult ScoreDebugBuffer(
        string testName,
        string source,
        byte[] pixels,
        int width,
        int height,
        string notes = "")
    {
        return RunVariant(testName, source, pixels, width, height, notes);
    }

    public RealScanQualityDiagnosticResult ScoreOriginal(FingerprintImage image, string source, string notes = "")
    {
        return RunVariant("Original captured buffer", source, image.Pixels, image.Width, image.Height, notes);
    }

    public RealScanQualityDiagnosticResult ScoreInverted(FingerprintImage image, string source, string notes = "")
    {
        var inverted = Invert(image.Pixels);
        return RunVariant("Inverted captured buffer", source, inverted, image.Width, image.Height, notes);
    }

    public RealScanQualityDiagnosticResult ScoreCropped(FingerprintImage image, string source, string notes = "")
    {
        var cropped = CropFingerprintBoundingBox(image.Pixels, image.Width, image.Height);
        return RunVariant("Cropped fingerprint bounding box", source, cropped.Pixels, cropped.Width, cropped.Height, notes);
    }

    private static RealScanQualityDiagnosticResult RunVariant(string name, byte[] pixels, int width, int height)
    {
        return RunVariant(name, string.Empty, pixels, width, height, string.Empty);
    }

    private static RealScanQualityDiagnosticResult RunVariant(string name, string source, byte[] pixels, int width, int height, string notes)
    {
        var stats = CalculateStats(pixels);
        var score = 0;
        var sdkResult = RunQualityScore(pixels, width, height, ref score);
        var message = sdkResult == RealScanSDK.RS_SUCCESS ? "OK" : GetErrorMessage(sdkResult);
        var status = sdkResult == RealScanSDK.RS_SUCCESS ? "OK" : "SDK Error";
        var combinedNotes = BuildNotes(notes, pixels, width, height, stats, sdkResult, score, message);

        var result = new RealScanQualityDiagnosticResult(
            name,
            source,
            width,
            height,
            pixels.Length,
            stats.Min,
            stats.Max,
            stats.Average,
            sdkResult,
            score,
            status,
            combinedNotes,
            message);

        LogDiagnostic(result);
        return result;
    }

    private async Task<RealScanQualityModeResult> SetQualityModeForScoringAsync(CancellationToken cancellationToken)
    {
        if (_scannerDiagnostics is null)
        {
            return new RealScanQualityModeResult(
                RealScanSDK.RS_SUCCESS,
                PreferredQualityMode,
                "No RealScan device handle was provided to the quality service; quality mode was not changed.");
        }

        if (!_scannerDiagnostics.IsDeviceConnected)
        {
            return new RealScanQualityModeResult(
                -1,
                PreferredQualityMode,
                "RealScan device is not connected; quality mode was not changed.");
        }

        try
        {
            return await _scannerDiagnostics.SetQualityModeAsync(PreferredQualityMode, cancellationToken);
        }
        catch (Exception ex)
        {
            return new RealScanQualityModeResult(-1, PreferredQualityMode, ex.Message);
        }
    }

    private static FingerprintQualityResult CreateQualityResult(int score)
    {
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

    private static string GetImageSource(FingerprintImage image, string fallback)
    {
        return string.IsNullOrWhiteSpace(image.Source) ? fallback : image.Source;
    }

    private static int RunQualityScore(byte[] pixels, int width, int height, ref int score)
    {
        IntPtr imageBuffer = IntPtr.Zero;
        try
        {
            imageBuffer = Marshal.AllocHGlobal(pixels.Length);
            Marshal.Copy(pixels, 0, imageBuffer, pixels.Length);
            return RealScanSDK.RS_GetQualityScore(imageBuffer, width, height, ref score);
        }
        finally
        {
            if (imageBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(imageBuffer);
            }
        }
    }

    private static byte[] Invert(byte[] pixels)
    {
        var inverted = new byte[pixels.Length];
        for (var index = 0; index < pixels.Length; index++)
        {
            inverted[index] = (byte)(255 - pixels[index]);
        }

        return inverted;
    }

    private static CroppedBuffer CropFingerprintBoundingBox(byte[] pixels, int width, int height)
    {
        var threshold = CalculateOtsuThreshold(pixels);
        var darkCount = pixels.Count(pixel => pixel <= threshold);
        var useDarkForeground = darkCount <= pixels.Length - darkCount;

        var left = width;
        var right = -1;
        var top = height;
        var bottom = -1;

        for (var y = 0; y < height; y++)
        {
            var rowOffset = y * width;
            for (var x = 0; x < width; x++)
            {
                var pixel = pixels[rowOffset + x];
                var isForeground = useDarkForeground ? pixel <= threshold : pixel > threshold;
                if (!isForeground)
                {
                    continue;
                }

                if (x < left) left = x;
                if (x > right) right = x;
                if (y < top) top = y;
                if (y > bottom) bottom = y;
            }
        }

        if (right < left || bottom < top)
        {
            return new CroppedBuffer(pixels.ToArray(), width, height);
        }

        const int margin = 24;
        left = Math.Max(0, left - margin);
        top = Math.Max(0, top - margin);
        right = Math.Min(width - 1, right + margin);
        bottom = Math.Min(height - 1, bottom + margin);

        var croppedWidth = right - left + 1;
        var croppedHeight = bottom - top + 1;
        if (croppedWidth <= 0 || croppedHeight <= 0)
        {
            return new CroppedBuffer(pixels.ToArray(), width, height);
        }

        var cropped = new byte[croppedWidth * croppedHeight];
        for (var y = 0; y < croppedHeight; y++)
        {
            Buffer.BlockCopy(
                pixels,
                ((top + y) * width) + left,
                cropped,
                y * croppedWidth,
                croppedWidth);
        }

        return new CroppedBuffer(cropped, croppedWidth, croppedHeight);
    }

    private static int CalculateOtsuThreshold(byte[] pixels)
    {
        var histogram = new int[256];
        foreach (var pixel in pixels)
        {
            histogram[pixel]++;
        }

        var total = pixels.Length;
        var sum = 0d;
        for (var value = 0; value < histogram.Length; value++)
        {
            sum += value * histogram[value];
        }

        var sumBackground = 0d;
        var weightBackground = 0;
        var maxVariance = 0d;
        var threshold = 127;

        for (var value = 0; value < histogram.Length; value++)
        {
            weightBackground += histogram[value];
            if (weightBackground == 0)
            {
                continue;
            }

            var weightForeground = total - weightBackground;
            if (weightForeground == 0)
            {
                break;
            }

            sumBackground += value * histogram[value];
            var meanBackground = sumBackground / weightBackground;
            var meanForeground = (sum - sumBackground) / weightForeground;
            var variance = weightBackground * weightForeground * Math.Pow(meanBackground - meanForeground, 2);

            if (variance > maxVariance)
            {
                maxVariance = variance;
                threshold = value;
            }
        }

        return threshold;
    }

    private static PixelStats CalculateStats(byte[] pixels)
    {
        if (pixels.Length == 0)
        {
            return new PixelStats(0, 0, 0);
        }

        var min = 255;
        var max = 0;
        long sum = 0;

        foreach (var pixel in pixels)
        {
            if (pixel < min) min = pixel;
            if (pixel > max) max = pixel;
            sum += pixel;
        }

        return new PixelStats(min, max, (double)sum / pixels.Length);
    }

    private static void LogDiagnostic(RealScanQualityDiagnosticResult result)
    {
        var line =
            $"RealScan quality diagnostic | test={result.TestName} | source={result.Source} | width={result.Width} | height={result.Height} | " +
            $"byteLength={result.ByteLength} | min={result.MinPixelValue} | max={result.MaxPixelValue} | " +
            $"avg={result.AveragePixelValue:F2} | sdkResult={result.SdkResult} | qualityScore={result.QualityScore} | status={result.Status} | notes={result.Notes} | message={result.Message}";

        Debug.WriteLine(line);
        Trace.WriteLine(line);
    }

    private static string BuildNotes(
        string initialNotes,
        byte[] pixels,
        int width,
        int height,
        PixelStats stats,
        int sdkResult,
        int score,
        string sdkMessage)
    {
        var notes = new List<string>();
        if (!string.IsNullOrWhiteSpace(initialNotes))
        {
            notes.Add(initialNotes);
        }

        if (pixels.Length != width * height)
        {
            notes.Add("Warning: byte length does not equal width * height.");
        }

        if (stats.Average > 230)
        {
            notes.Add("Image may be too light / mostly background.");
        }

        if (stats.Average < 25)
        {
            notes.Add("Image may be inverted or too dark.");
        }

        if (sdkResult != RealScanSDK.RS_SUCCESS)
        {
            notes.Add(sdkMessage);
        }

        if (score < 5)
        {
            notes.Add("Compare with official RealScan sample app quality score.");
        }

        return notes.Count == 0 ? "None" : string.Join(" ", notes);
    }

    private static string GetErrorMessage(int errorCode)
    {
        var message = string.Empty;
        try
        {
            RealScanSDK.RS_GetErrString(errorCode, ref message);
        }
        catch
        {
            message = string.Empty;
        }

        return string.IsNullOrWhiteSpace(message) ? $"RealScan quality error {errorCode}." : message.Trim('\0', ' ', '\r', '\n');
    }

    private sealed record PixelStats(int Min, int Max, double Average);

    private sealed record CroppedBuffer(byte[] Pixels, int Width, int Height);
}

public sealed record RealScanQualityDiagnosticResult(
    string TestName,
    string Source,
    int Width,
    int Height,
    int ByteLength,
    int MinPixelValue,
    int MaxPixelValue,
    double AveragePixelValue,
    int SdkResult,
    int QualityScore,
    string Status,
    string Notes,
    string Message);
