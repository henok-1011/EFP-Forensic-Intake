using EFP_forensic.Domain.Enums;
using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;
using RealScan;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EFP_forensic.Services.Scanner;

public sealed class RealScanSdkFingerprintScanner : IFingerprintScanner, IRealScanScannerDiagnostics
{
    private readonly ConcurrentQueue<FingerprintFrame> _previewFrames = new();
    private RSPreviewDataCallback? _previewCallback;
    private int _deviceHandle;
    private bool _sdkInitialized;
    private bool _deviceConnected;
    private string _lastError = string.Empty;
    private FingerprintFrame? _lastPreviewFrame;

    public IReadOnlyList<RealScanDeviceSummary> Devices { get; private set; } = Array.Empty<RealScanDeviceSummary>();

    public string LastError => _lastError;

    public bool IsSdkInitialized => _sdkInitialized;

    public bool IsDeviceConnected => _deviceConnected;

    public RealScanDeviceSummary? ConnectedDevice { get; private set; }

    public FingerprintFrame? LastPreviewFrame => _lastPreviewFrame;

    public Task<RealScanDiagnosticResult> InitializeSdkAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            var count = 0;
            var result = RealScanSDK.RS_InitSDK(null!, 0, ref count);
            if (result != RealScanSDK.RS_SUCCESS && result != RealScanSDK.RS_ERR_SDK_ALREADY_INITIALIZED)
            {
                return Fail(result, "SDK initialization failed.");
            }

            _sdkInitialized = true;
            return RealScanDiagnosticResult.Ok($"SDK initialized. Device count reported: {count}.");
        }, cancellationToken);
    }

    public async Task<IReadOnlyList<RealScanDeviceSummary>> EnumerateDevicesAsync(CancellationToken cancellationToken)
    {
        if (!_sdkInitialized)
        {
            var init = await InitializeSdkAsync(cancellationToken);
            if (!init.Success)
            {
                Devices = Array.Empty<RealScanDeviceSummary>();
                return Devices;
            }
        }

        return await Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            var devices = new List<RealScanDeviceSummary>();
            var count = 0;
            var result = RealScanSDK.RS_GetNumOfDevice(ref count);
            if (result != RealScanSDK.RS_SUCCESS)
            {
                _lastError = GetErrorMessage(result);
                Devices = Array.Empty<RealScanDeviceSummary>();
                return Devices;
            }

            for (var index = 0; index < count; index++)
            {
                var deviceId = -1;
                if (RealScanSDK.RS_GetDeviceId(index, ref deviceId) != RealScanSDK.RS_SUCCESS)
                {
                    continue;
                }

                var handle = -1;
                var deviceType = 0;
                var deviceStatus = 0;
                RealScanSDK.RS_GetDeviceStatus(deviceId, ref handle, ref deviceType, ref deviceStatus);
                devices.Add(new RealScanDeviceSummary(index, deviceId, handle, deviceType, deviceStatus, GetDeviceName(deviceType)));
            }

            Devices = devices;
            return Devices;
        }, cancellationToken);
    }

    public async Task<RealScanDeviceInfo?> GetConnectedDeviceInfoAsync(CancellationToken cancellationToken)
    {
        if (!_deviceConnected)
        {
            return null;
        }

        return await Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            var info = new RSDeviceInfo();
            var result = RealScanSDK.RS_GetDeviceInfo(_deviceHandle, ref info);
            if (result != RealScanSDK.RS_SUCCESS)
            {
                _lastError = GetErrorMessage(result);
                return null;
            }

            return new RealScanDeviceInfo(
                GetDeviceName(info.deviceType),
                ReadAnsi(info.productName),
                ReadAnsi(info.deviceID),
                ReadAnsi(info.firmwareVersion),
                ReadAnsi(info.HardwareVersion),
                info.deviceType);
        }, cancellationToken);
    }

    public Task ConnectAsync(CancellationToken cancellationToken)
    {
        return ConnectAsync(0, cancellationToken);
    }

    public async Task ConnectAsync(int deviceListIndex, CancellationToken cancellationToken)
    {
        if (!_sdkInitialized)
        {
            var init = await InitializeSdkAsync(cancellationToken);
            if (!init.Success)
            {
                throw new InvalidOperationException(init.Message);
            }
        }

        if (Devices.Count == 0)
        {
            await EnumerateDevicesAsync(cancellationToken);
        }

        if (Devices.Count == 0)
        {
            throw new InvalidOperationException("No RealScan devices were found.");
        }

        var selected = Devices[Math.Clamp(deviceListIndex, 0, Devices.Count - 1)];
        var handle = 0;
        var result = await Task.Run(() => RealScanSDK.RS_InitDevice(selected.DeviceId, ref handle), cancellationToken);
        if (result != RealScanSDK.RS_SUCCESS)
        {
            throw new InvalidOperationException(GetErrorMessage(result));
        }

        _deviceHandle = handle;
        _deviceConnected = true;
        ConnectedDevice = selected with { DeviceHandle = handle };
    }

    public Task DisconnectAsync()
    {
        StopPreview();

        if (_deviceConnected)
        {
            RealScanSDK.RS_AbortCapture(_deviceHandle);
            RealScanSDK.RS_ExitDevice(_deviceHandle);
        }

        _deviceHandle = 0;
        _deviceConnected = false;
        ConnectedDevice = null;
        return Task.CompletedTask;
    }

    public async IAsyncEnumerable<FingerprintFrame> StreamPreviewAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        EnsureConnected();

        _previewFrames.Clear();
        _previewCallback = PreviewDataCallback;
        var result = RealScanSDK.RS_RegisterPreviewCallback(_deviceHandle, _previewCallback);
        if (result != RealScanSDK.RS_SUCCESS)
        {
            throw new InvalidOperationException(GetErrorMessage(result));
        }

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!_deviceConnected)
                {
                    throw new InvalidOperationException("Scanner disconnected. Please reconnect the device.");
                }

                while (_previewFrames.TryDequeue(out var frame))
                {
                    yield return frame;
                }

                await Task.Delay(33, cancellationToken);
            }
        }
        finally
        {
            StopPreview();
        }
    }

    public Task<FingerprintCaptureResult> CaptureAsync(FingerPosition finger, CaptureMode mode, CancellationToken cancellationToken)
    {
        EnsureConnected();

        return Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            var captureMode = MapCaptureMode(mode);
            var modeResult = RealScanSDK.RS_SetCaptureMode(_deviceHandle, captureMode, RealScanSDK.RS_AUTO_SENSITIVITY_NORMAL, true);
            if (modeResult != RealScanSDK.RS_SUCCESS)
            {
                MarkDisconnectedIfNeeded(modeResult);
                return new FingerprintCaptureResult { Success = false, ErrorMessage = GetErrorMessage(modeResult) };
            }

            IntPtr imageData = IntPtr.Zero;
            var width = 0;
            var height = 0;

            try
            {
                var result = RealScanSDK.RS_TakeImageData(_deviceHandle, 10000, ref imageData, ref width, ref height);
                if (result != RealScanSDK.RS_SUCCESS || imageData == IntPtr.Zero || width <= 0 || height <= 0)
                {
                    MarkDisconnectedIfNeeded(result);
                    return new FingerprintCaptureResult
                    {
                        Success = false,
                        ErrorMessage = result == RealScanSDK.RS_SUCCESS ? "Capture returned no image data." : GetErrorMessage(result)
                    };
                }

                var pixels = CopyImage(imageData, width, height);
                return new FingerprintCaptureResult
                {
                    Success = true,
                    Image = new FingerprintImage
                    {
                        Pixels = pixels,
                        Width = width,
                        Height = height,
                        Dpi = 500,
                        Source = "RS_TakeImageData",
                        CaptureSdkResult = result,
                        FingerPosition = finger,
                        CaptureMode = mode,
                        CapturedAt = DateTimeOffset.UtcNow
                    }
                };
            }
            finally
            {
                if (imageData != IntPtr.Zero)
                {
                    RealScanSDK.RS_FreeImageData(imageData);
                }
            }
        }, cancellationToken);
    }

    public Task<SlapCaptureResult> CaptureSlapGroupAsync(SlapCaptureGroup group, CancellationToken cancellationToken)
    {
        EnsureConnected();

        return Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            var captureMode = MapSlapCaptureMode(group);
            var slapType = MapSlapType(group);
            var fingerPositions = GetSlapFingerPositions(group);

            var modeResult = RealScanSDK.RS_SetCaptureMode(_deviceHandle, captureMode, RealScanSDK.RS_AUTO_SENSITIVITY_NORMAL, true);
            if (modeResult != RealScanSDK.RS_SUCCESS)
            {
                MarkDisconnectedIfNeeded(modeResult);
                return new SlapCaptureResult
                {
                    Success = false,
                    Group = group,
                    SdkResult = modeResult,
                    ErrorMessage = GetErrorMessage(modeResult)
                };
            }

            IntPtr slapImageData = IntPtr.Zero;
            var slapWidth = 0;
            var slapHeight = 0;

            IntPtr slapInfo = IntPtr.Zero;
            IntPtr finger1 = IntPtr.Zero;
            IntPtr finger2 = IntPtr.Zero;
            IntPtr finger3 = IntPtr.Zero;
            IntPtr finger4 = IntPtr.Zero;
            var finger1Width = 0;
            var finger1Height = 0;
            var finger2Width = 0;
            var finger2Height = 0;
            var finger3Width = 0;
            var finger3Height = 0;
            var finger4Width = 0;
            var finger4Height = 0;
            var numOfFinger = 0;

            try
            {
                var captureResult = RealScanSDK.RS_TakeImageData(_deviceHandle, 10000, ref slapImageData, ref slapWidth, ref slapHeight);
                if (captureResult != RealScanSDK.RS_SUCCESS || slapImageData == IntPtr.Zero || slapWidth <= 0 || slapHeight <= 0)
                {
                    MarkDisconnectedIfNeeded(captureResult);
                    return new SlapCaptureResult
                    {
                        Success = false,
                        Group = group,
                        SdkResult = captureResult,
                        ErrorMessage = captureResult == RealScanSDK.RS_SUCCESS ? "Slap capture returned no image data." : GetErrorMessage(captureResult)
                    };
                }

                var segmentResult = RealScanSDK.RS_Segment4(
                    slapImageData,
                    slapWidth,
                    slapHeight,
                    slapType,
                    ref numOfFinger,
                    ref slapInfo,
                    ref finger1,
                    ref finger1Width,
                    ref finger1Height,
                    ref finger2,
                    ref finger2Width,
                    ref finger2Height,
                    ref finger3,
                    ref finger3Width,
                    ref finger3Height,
                    ref finger4,
                    ref finger4Width,
                    ref finger4Height);

                if (segmentResult != RealScanSDK.RS_SUCCESS)
                {
                    MarkDisconnectedIfNeeded(segmentResult);
                    return new SlapCaptureResult
                    {
                        Success = false,
                        Group = group,
                        SdkResult = segmentResult,
                        ErrorMessage = GetErrorMessage(segmentResult)
                    };
                }

                var segmentPointers = new[]
                {
                    (Pointer: finger1, Width: finger1Width, Height: finger1Height),
                    (Pointer: finger2, Width: finger2Width, Height: finger2Height),
                    (Pointer: finger3, Width: finger3Width, Height: finger3Height),
                    (Pointer: finger4, Width: finger4Width, Height: finger4Height)
                };

                var expectedCount = fingerPositions.Length;
                var availableCount = Math.Min(Math.Min(numOfFinger, expectedCount), segmentPointers.Length);
                if (availableCount < expectedCount)
                {
                    return new SlapCaptureResult
                    {
                        Success = false,
                        Group = group,
                        SdkResult = RealScanSDK.RS_ERR_SEGMENT_FEWER_FINGER,
                        ErrorMessage = $"Segmentation returned {availableCount} finger(s); expected {expectedCount}."
                    };
                }

                var segments = new List<SegmentedFingerprintImage>(expectedCount);
                for (var index = 0; index < expectedCount; index++)
                {
                    var segment = segmentPointers[index];
                    if (segment.Pointer == IntPtr.Zero || segment.Width <= 0 || segment.Height <= 0)
                    {
                        return new SlapCaptureResult
                        {
                            Success = false,
                            Group = group,
                            SdkResult = RealScanSDK.RS_ERR_CANNOT_SEGMENT,
                            ErrorMessage = $"Segmentation returned empty image data for {fingerPositions[index]}."
                        };
                    }

                    segments.Add(new SegmentedFingerprintImage
                    {
                        FingerPosition = fingerPositions[index],
                        Image = new FingerprintImage
                        {
                            Pixels = CopyImage(segment.Pointer, segment.Width, segment.Height),
                            Width = segment.Width,
                            Height = segment.Height,
                            Dpi = 500,
                            Source = $"RS_Segment4/{group}",
                            CaptureSdkResult = segmentResult,
                            FingerPosition = fingerPositions[index],
                            CaptureMode = CaptureMode.Slap442,
                            CapturedAt = DateTimeOffset.UtcNow
                        }
                    });
                }

                return new SlapCaptureResult
                {
                    Success = true,
                    Group = group,
                    Segments = segments,
                    SdkResult = segmentResult
                };
            }
            finally
            {
                FreeSdkImageData(finger1);
                FreeSdkImageData(finger2);
                FreeSdkImageData(finger3);
                FreeSdkImageData(finger4);
                FreeSdkImageData(slapInfo);
                FreeSdkImageData(slapImageData);
            }
        }, cancellationToken);
    }

    public Task<RealScanQualityModeResult> SetQualityModeAsync(int qualityMode, CancellationToken cancellationToken)
    {
        EnsureConnected();

        return Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            var requestedMode = qualityMode;
            var result = RealScanSDK.RS_SetParam(_deviceHandle, RealScanSDK.RS_GVAR_FP_QUALITY_MODE, ref requestedMode);
            return new RealScanQualityModeResult(
                result,
                requestedMode,
                result == RealScanSDK.RS_SUCCESS ? "OK" : GetErrorMessage(result));
        }, cancellationToken);
    }

    public Task<RealScanRawImageResult> CaptureSdkFinalImageAsync(CancellationToken cancellationToken)
    {
        EnsureConnected();

        return Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            IntPtr imageData = IntPtr.Zero;
            var width = 0;
            var height = 0;

            try
            {
                var result = RealScanSDK.RS_TakeImageData(_deviceHandle, 10000, ref imageData, ref width, ref height);
                if (result != RealScanSDK.RS_SUCCESS || imageData == IntPtr.Zero || width <= 0 || height <= 0)
                {
                    return new RealScanRawImageResult(
                        false,
                        "RS_TakeImageData",
                        [],
                        width,
                        height,
                        result,
                        result == RealScanSDK.RS_SUCCESS ? "Capture returned no image data." : GetErrorMessage(result));
                }

                return new RealScanRawImageResult(
                    true,
                    "RS_TakeImageData",
                    CopyImage(imageData, width, height),
                    width,
                    height,
                    result,
                    "OK");
            }
            finally
            {
                if (imageData != IntPtr.Zero)
                {
                    RealScanSDK.RS_FreeImageData(imageData);
                }
            }
        }, cancellationToken);
    }

    public Task<RealScanRawImageResult> TakeCurrentImageDataAsync(CancellationToken cancellationToken)
    {
        EnsureConnected();

        return Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            IntPtr imageData = IntPtr.Zero;
            var width = 0;
            var height = 0;

            try
            {
                var result = RealScanSDK.RS_TakeCurrentImageData(_deviceHandle, 5000, ref imageData, ref width, ref height);
                if (result != RealScanSDK.RS_SUCCESS || imageData == IntPtr.Zero || width <= 0 || height <= 0)
                {
                    return new RealScanRawImageResult(
                        false,
                        "RS_TakeCurrentImageData",
                        [],
                        width,
                        height,
                        result,
                        result == RealScanSDK.RS_SUCCESS ? "Current image returned no image data." : GetErrorMessage(result));
                }

                return new RealScanRawImageResult(
                    true,
                    "RS_TakeCurrentImageData",
                    CopyImage(imageData, width, height),
                    width,
                    height,
                    result,
                    "OK");
            }
            finally
            {
                if (imageData != IntPtr.Zero)
                {
                    RealScanSDK.RS_FreeImageData(imageData);
                }
            }
        }, cancellationToken);
    }

    public void StopPreview()
    {
        if (_deviceConnected)
        {
            RealScanSDK.RS_RegisterPreviewCallback(_deviceHandle, null!);
        }

        _previewCallback = null;
    }

    private void PreviewDataCallback(int deviceHandle, int errorCode, IntPtr imageData, int imageWidth, int imageHeight)
    {
        if (errorCode != RealScanSDK.RS_SUCCESS || imageData == IntPtr.Zero || imageWidth <= 0 || imageHeight <= 0)
        {
            if (errorCode != RealScanSDK.RS_SUCCESS)
            {
                _lastError = GetErrorMessage(errorCode);
                MarkDisconnectedIfNeeded(errorCode);
            }

            return;
        }

        var pixels = CopyImage(imageData, imageWidth, imageHeight);
        var frame = new FingerprintFrame
        {
            Pixels = pixels,
            Width = imageWidth,
            Height = imageHeight,
            Dpi = 500,
            Timestamp = DateTimeOffset.UtcNow
        };
        _lastPreviewFrame = frame;
        _previewFrames.Enqueue(frame);
    }

    private void EnsureConnected()
    {
        if (!_deviceConnected)
        {
            throw new InvalidOperationException("RealScan scanner is not connected.");
        }
    }

    private static byte[] CopyImage(IntPtr imageData, int imageWidth, int imageHeight)
    {
        var length = checked(imageWidth * imageHeight);
        var pixels = new byte[length];
        Marshal.Copy(imageData, pixels, 0, length);
        return pixels;
    }

    private static int MapCaptureMode(CaptureMode mode)
    {
        return mode switch
        {
            CaptureMode.Rolled => RealScanSDK.RS_CAPTURE_ROLL_FINGER,
            _ => RealScanSDK.RS_CAPTURE_FLAT_SINGLE_FINGER
        };
    }

    private static int MapSlapCaptureMode(SlapCaptureGroup group)
    {
        return group switch
        {
            SlapCaptureGroup.RightFour => RealScanSDK.RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS,
            SlapCaptureGroup.LeftFour => RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS,
            SlapCaptureGroup.Thumbs => RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS,
            _ => throw new ArgumentOutOfRangeException(nameof(group), group, "Unsupported slap capture group.")
        };
    }

    private static int MapSlapType(SlapCaptureGroup group)
    {
        return group switch
        {
            SlapCaptureGroup.RightFour => RealScanSDK.RS_SLAP_RIGHT_FOUR,
            SlapCaptureGroup.LeftFour => RealScanSDK.RS_SLAP_LEFT_FOUR,
            SlapCaptureGroup.Thumbs => RealScanSDK.RS_SLAP_TWO_THUMB,
            _ => throw new ArgumentOutOfRangeException(nameof(group), group, "Unsupported slap capture group.")
        };
    }

    private static FingerPosition[] GetSlapFingerPositions(SlapCaptureGroup group)
    {
        return group switch
        {
            SlapCaptureGroup.RightFour =>
            [
                FingerPosition.RightIndex,
                FingerPosition.RightMiddle,
                FingerPosition.RightRing,
                FingerPosition.RightLittle
            ],
            SlapCaptureGroup.LeftFour =>
            [
                FingerPosition.LeftIndex,
                FingerPosition.LeftMiddle,
                FingerPosition.LeftRing,
                FingerPosition.LeftLittle
            ],
            SlapCaptureGroup.Thumbs =>
            [
                FingerPosition.LeftThumb,
                FingerPosition.RightThumb
            ],
            _ => []
        };
    }

    private void MarkDisconnectedIfNeeded(int sdkResult)
    {
        if (!IsDisconnectResult(sdkResult))
        {
            return;
        }

        _deviceConnected = false;
        ConnectedDevice = null;
        _lastError = "Scanner disconnected. Please reconnect the device.";
    }

    private static bool IsDisconnectResult(int sdkResult)
    {
        return sdkResult is RealScanSDK.RS_ERR_NO_DEVICE
            or RealScanSDK.RS_ERR_CANNOT_GET_USB_DEVICE
            or RealScanSDK.RS_ERR_CANNOT_WRITE_USB
            or RealScanSDK.RS_ERR_WRITE_USB_TIMEOUT
            or RealScanSDK.RS_ERR_CANNOT_READ_USB
            or RealScanSDK.RS_ERR_READ_USB_TIMEOUT
            or RealScanSDK.RS_ERR_INVALID_DEVICE_CONNECTION
            or RealScanSDK.RS_ERR_DEVICE_NOT_INITIALIZED;
    }

    private static void FreeSdkImageData(IntPtr imageData)
    {
        if (imageData != IntPtr.Zero)
        {
            RealScanSDK.RS_FreeImageData(imageData);
        }
    }

    private static RealScanDiagnosticResult Fail(int errorCode, string fallback)
    {
        var message = GetErrorMessage(errorCode);
        return new RealScanDiagnosticResult(false, string.IsNullOrWhiteSpace(message) ? fallback : message);
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

        return string.IsNullOrWhiteSpace(message) ? $"RealScan SDK error {errorCode}." : message.Trim('\0', ' ', '\r', '\n');
    }

    private static string ReadAnsi(byte[]? bytes)
    {
        if (bytes is null || bytes.Length == 0)
        {
            return string.Empty;
        }

        var end = Array.IndexOf(bytes, (byte)0);
        if (end < 0)
        {
            end = bytes.Length;
        }

        return System.Text.Encoding.ASCII.GetString(bytes, 0, end).Trim();
    }

    private static string GetDeviceName(int deviceType)
    {
        return deviceType switch
        {
            RealScanSDK.RS_DEVICE_REALSCAN_10 => "RealScan-10",
            RealScanSDK.RS_DEVICE_REALSCAN_10F => "RealScan-10F",
            RealScanSDK.RS_DEVICE_REALSCAN_D => "RealScan-D",
            RealScanSDK.RS_DEVICE_REALSCAN_DF => "RealScan-DF",
            RealScanSDK.RS_DEVICE_REALSCAN_F => "RealScan-F",
            RealScanSDK.RS_DEVICE_REALSCAN_G10 => "RealScan-G10",
            RealScanSDK.RS_DEVICE_REALSCAN_G10F => "RealScan-G10F",
            RealScanSDK.RS_DEVICE_REALSCAN_G10I => "RealScan-G10i",
            RealScanSDK.RS_DEVICE_REALSCAN_S60 => "RealScan-S60",
            RealScanSDK.RS_DEVICE_REALSCAN_SG10 => "RealScan-SG10",
            RealScanSDK.RS_DEVICE_REALSCAN_SG10I => "RealScan-SG10i",
            _ => $"RealScan device type {deviceType}"
        };
    }
}

public sealed record RealScanDeviceSummary(
    int ListIndex,
    int DeviceId,
    int DeviceHandle,
    int DeviceType,
    int DeviceStatus,
    string DeviceName)
{
    public string DisplayName => $"{ListIndex}: {DeviceName} (ID {DeviceId}, Status {DeviceStatus})";
}

public sealed record RealScanDeviceInfo(
    string DeviceName,
    string ProductName,
    string DeviceId,
    string FirmwareVersion,
    string HardwareVersion,
    int DeviceType);

public sealed record RealScanDiagnosticResult(bool Success, string Message)
{
    public static RealScanDiagnosticResult Ok(string message) => new(true, message);
}

public sealed record RealScanQualityModeResult(int SdkResult, int RequestedMode, string Message);

public sealed record RealScanRawImageResult(
    bool Success,
    string Source,
    byte[] Pixels,
    int Width,
    int Height,
    int SdkResult,
    string Message);
