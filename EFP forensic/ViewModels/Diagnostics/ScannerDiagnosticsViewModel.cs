using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EFP_forensic.Domain.Enums;
using EFP_forensic.Helpers;
using EFP_forensic.Models;
using EFP_forensic.Services.Forensics;
using EFP_forensic.Services.Scanner;

namespace EFP_forensic.ViewModels.Diagnostics;

public sealed class ScannerDiagnosticsViewModel : ObservableObject
{
    private readonly RealScanSdkFingerprintScanner _scanner;
    private readonly RealScanQualityService _qualityService;
    private readonly RealScanWsqEncodingService _wsqEncodingService;
    private CancellationTokenSource? _previewCancellation;
    private RealScanDeviceSummary? _selectedDevice;
    private RealScanDeviceInfo? _connectedDeviceInfo;
    private FingerprintImage? _capturedImage;
    private FingerprintImage? _sdkFinalImage;
    private ImageSource? _previewImage;
    private ImageSource? _capturedImageSource;
    private string _statusMessage = "Scanner diagnostics ready.";
    private string _qualityScore = "Not run";
    private string _wsqByteLength = "Not encoded";
    private bool _isBusy;
    private bool _isPreviewing;

    public ScannerDiagnosticsViewModel(
        RealScanSdkFingerprintScanner scanner,
        RealScanQualityService qualityService,
        RealScanWsqEncodingService wsqEncodingService)
    {
        _scanner = scanner;
        _qualityService = qualityService;
        _wsqEncodingService = wsqEncodingService;

        Devices = [];
        QualityDiagnosticResults = [];

        InitializeSdkCommand = new AsyncRelayCommand(InitializeSdkAsync, () => !IsBusy);
        EnumerateDevicesCommand = new AsyncRelayCommand(EnumerateDevicesAsync, () => !IsBusy);
        ConnectScannerCommand = new AsyncRelayCommand(ConnectScannerAsync, () => !IsBusy && SelectedDevice is not null);
        StartPreviewCommand = new AsyncRelayCommand(StartPreviewAsync, () => !IsBusy && IsConnected && !IsPreviewing);
        StopPreviewCommand = new RelayCommand(StopPreview, () => IsPreviewing);
        CaptureTestFingerCommand = new AsyncRelayCommand(CaptureTestFingerAsync, () => !IsBusy && IsConnected);
        RunQualityCommand = new AsyncRelayCommand(RunQualityAsync, () => !IsBusy && CapturedImage is not null);
        RunQualityDiagnosticsCommand = new AsyncRelayCommand(RunQualityDiagnosticsAsync, () => !IsBusy && CapturedImage is not null);
        CaptureSdkFinalImageCommand = new AsyncRelayCommand(CaptureSdkFinalImageAsync, () => !IsBusy && IsConnected);
        ScoreLastCapturedImageCommand = new AsyncRelayCommand(ScoreLastCapturedImageAsync, () => !IsBusy && CapturedImage is not null);
        ClearQualityDebugResultsCommand = new RelayCommand(ClearQualityDebugResults, () => QualityDiagnosticResults.Count > 0);
        EncodeWsqCommand = new AsyncRelayCommand(EncodeWsqAsync, () => !IsBusy && CapturedImage is not null);
        DisconnectCommand = new AsyncRelayCommand(DisconnectAsync, () => !IsBusy && IsConnected);
    }

    public ObservableCollection<RealScanDeviceSummary> Devices { get; }

    public ObservableCollection<RealScanQualityDiagnosticResult> QualityDiagnosticResults { get; }

    public RealScanDeviceSummary? SelectedDevice
    {
        get => _selectedDevice;
        set
        {
            if (SetProperty(ref _selectedDevice, value))
            {
                ConnectScannerCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public RealScanDeviceInfo? ConnectedDeviceInfo
    {
        get => _connectedDeviceInfo;
        private set
        {
            if (SetProperty(ref _connectedDeviceInfo, value))
            {
                OnPropertyChanged(nameof(DeviceInfoText));
            }
        }
    }

    public string DeviceInfoText => ConnectedDeviceInfo is null
        ? "No scanner connected."
        : $"Product: {ConnectedDeviceInfo.ProductName}\nDevice ID: {ConnectedDeviceInfo.DeviceId}\nFirmware: {ConnectedDeviceInfo.FirmwareVersion}\nHardware: {ConnectedDeviceInfo.HardwareVersion}\nType: {ConnectedDeviceInfo.DeviceName}";

    public ImageSource? PreviewImage
    {
        get => _previewImage;
        private set => SetProperty(ref _previewImage, value);
    }

    public ImageSource? CapturedImageSource
    {
        get => _capturedImageSource;
        private set => SetProperty(ref _capturedImageSource, value);
    }

    public FingerprintImage? CapturedImage
    {
        get => _capturedImage;
        private set
        {
            if (SetProperty(ref _capturedImage, value))
            {
                RunQualityCommand.NotifyCanExecuteChanged();
                RunQualityDiagnosticsCommand.NotifyCanExecuteChanged();
                EncodeWsqCommand.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(CapturedImageText));
            }
        }
    }

    public string CapturedImageText => CapturedImage is null
        ? "No captured image."
        : $"{CapturedImage.Width} x {CapturedImage.Height}, {CapturedImage.Pixels.Length:N0} bytes";

    public string StatusMessage
    {
        get => _statusMessage;
        private set => SetProperty(ref _statusMessage, value);
    }

    public string QualityScore
    {
        get => _qualityScore;
        private set => SetProperty(ref _qualityScore, value);
    }

    public string WsqByteLength
    {
        get => _wsqByteLength;
        private set => SetProperty(ref _wsqByteLength, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (SetProperty(ref _isBusy, value))
            {
                RefreshCommands();
            }
        }
    }

    public bool IsPreviewing
    {
        get => _isPreviewing;
        private set
        {
            if (SetProperty(ref _isPreviewing, value))
            {
                RefreshCommands();
            }
        }
    }

    public bool IsConnected => _scanner.IsDeviceConnected;

    public AsyncRelayCommand InitializeSdkCommand { get; }

    public AsyncRelayCommand EnumerateDevicesCommand { get; }

    public AsyncRelayCommand ConnectScannerCommand { get; }

    public AsyncRelayCommand StartPreviewCommand { get; }

    public RelayCommand StopPreviewCommand { get; }

    public AsyncRelayCommand CaptureTestFingerCommand { get; }

    public AsyncRelayCommand RunQualityCommand { get; }

    public AsyncRelayCommand RunQualityDiagnosticsCommand { get; }

    public AsyncRelayCommand CaptureSdkFinalImageCommand { get; }

    public AsyncRelayCommand ScoreLastCapturedImageCommand { get; }

    public RelayCommand ClearQualityDebugResultsCommand { get; }

    public AsyncRelayCommand EncodeWsqCommand { get; }

    public AsyncRelayCommand DisconnectCommand { get; }

    private async Task InitializeSdkAsync()
    {
        await RunBusyAsync(async () =>
        {
            var result = await _scanner.InitializeSdkAsync(CancellationToken.None);
            StatusMessage = result.Message;
        });
    }

    private async Task EnumerateDevicesAsync()
    {
        await RunBusyAsync(async () =>
        {
            Devices.Clear();
            var devices = await _scanner.EnumerateDevicesAsync(CancellationToken.None);
            foreach (var device in devices)
            {
                Devices.Add(device);
            }

            SelectedDevice = Devices.FirstOrDefault();
            StatusMessage = Devices.Count == 0
                ? "No RealScan devices found."
                : $"Enumerated {Devices.Count} RealScan device(s).";
        });
    }

    private async Task ConnectScannerAsync()
    {
        if (SelectedDevice is null)
        {
            StatusMessage = "Select a device before connecting.";
            return;
        }

        await RunBusyAsync(async () =>
        {
            await _scanner.ConnectAsync(SelectedDevice.ListIndex, CancellationToken.None);
            ConnectedDeviceInfo = await _scanner.GetConnectedDeviceInfoAsync(CancellationToken.None);
            StatusMessage = "Scanner connected.";
            OnPropertyChanged(nameof(IsConnected));
        });
    }

    private async Task StartPreviewAsync()
    {
        if (!IsConnected)
        {
            StatusMessage = "Connect scanner before starting preview.";
            return;
        }

        _previewCancellation?.Cancel();
        _previewCancellation = new CancellationTokenSource();
        IsPreviewing = true;
        StatusMessage = "Live preview started.";

        try
        {
            await foreach (var frame in _scanner.StreamPreviewAsync(_previewCancellation.Token))
            {
                PreviewImage = CreateBitmap(frame.Pixels, frame.Width, frame.Height);
            }
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "Live preview stopped.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Preview failed: {ex.Message}";
        }
        finally
        {
            IsPreviewing = false;
        }
    }

    private void StopPreview()
    {
        _previewCancellation?.Cancel();
        _previewCancellation?.Dispose();
        _previewCancellation = null;
        _scanner.StopPreview();
        IsPreviewing = false;
        StatusMessage = "Live preview stopped.";
    }

    private async Task CaptureTestFingerAsync()
    {
        await RunBusyAsync(async () =>
        {
            var result = await _scanner.CaptureAsync(FingerPosition.RightIndex, CaptureMode.Flat, CancellationToken.None);
            if (!result.Success || result.Image is null)
            {
                StatusMessage = string.IsNullOrWhiteSpace(result.ErrorMessage)
                    ? "Test capture failed."
                    : result.ErrorMessage;
                return;
            }

            CapturedImage = result.Image;
            _sdkFinalImage = result.Image;
            CapturedImageSource = CreateBitmap(result.Image.Pixels, result.Image.Width, result.Image.Height);
            QualityScore = "Not run";
            WsqByteLength = "Not encoded";
            QualityDiagnosticResults.Clear();
            StatusMessage = "Captured test finger image.";
        });
    }

    private async Task RunQualityAsync()
    {
        if (CapturedImage is null)
        {
            StatusMessage = "Capture an image before quality scoring.";
            return;
        }

        await RunBusyAsync(async () =>
        {
            var result = await _qualityService.AssessAsync(CapturedImage, CancellationToken.None);
            QualityScore = $"{result.Score}/100 - {result.Message}";
            StatusMessage = "Quality score completed.";
        });
    }

    private async Task RunQualityDiagnosticsAsync()
    {
        if (CapturedImage is null)
        {
            StatusMessage = "Capture an image before quality diagnostics.";
            return;
        }

        await RunBusyAsync(async () =>
        {
            QualityDiagnosticResults.Clear();
            await AddQualityDebugResultsAsync(CapturedImage, "RS_TakeImageData", "Main captured buffer. RS_GetQualityScore called as (imagePtr, width, height).");

            StatusMessage = "Quality diagnostics completed for original, inverted, and cropped buffers.";
        });
    }

    private async Task CaptureSdkFinalImageAsync()
    {
        await RunBusyAsync(async () =>
        {
            var qualityMode = await _scanner.SetQualityModeAsync(2, CancellationToken.None);
            AddQualityDebugResult(new RealScanQualityDiagnosticResult(
                "RS_SetParam quality mode",
                "RS_SetParam",
                0,
                0,
                0,
                0,
                0,
                0,
                qualityMode.SdkResult,
                qualityMode.RequestedMode,
                qualityMode.SdkResult == 0 ? "OK" : "SDK Error",
                $"RS_SetParam(deviceHandle, RS_GVAR_FP_QUALITY_MODE, ref iq), iq={qualityMode.RequestedMode}.",
                qualityMode.Message));

            var result = await _scanner.CaptureSdkFinalImageAsync(CancellationToken.None);
            if (!result.Success)
            {
                AddQualityDebugResult(new RealScanQualityDiagnosticResult(
                    "SDK final capture buffer",
                    result.Source,
                    result.Width,
                    result.Height,
                    result.Pixels.Length,
                    0,
                    0,
                    0,
                    result.SdkResult,
                    0,
                    "Capture Error",
                    result.Message,
                    result.Message));
                StatusMessage = result.Message;
                return;
            }

            _sdkFinalImage = new FingerprintImage
            {
                Pixels = result.Pixels,
                Width = result.Width,
                Height = result.Height,
                Dpi = 500,
                Source = result.Source,
                CaptureSdkResult = result.SdkResult,
                FingerPosition = FingerPosition.RightIndex,
                CaptureMode = CaptureMode.Flat,
                CapturedAt = DateTimeOffset.UtcNow
            };

            CapturedImage = _sdkFinalImage;
            CapturedImageSource = CreateBitmap(result.Pixels, result.Width, result.Height);
            AddQualityDebugResult(await _qualityService.ScoreCapturedImageAsync(_sdkFinalImage, result.Source, CancellationToken.None));
            StatusMessage = "Captured SDK final image and scored it.";
        });
    }

    private async Task ScoreLastCapturedImageAsync()
    {
        if (CapturedImage is null)
        {
            StatusMessage = "Capture an image before scoring.";
            return;
        }

        await RunBusyAsync(async () =>
        {
            await AddQualityDebugResultsAsync(CapturedImage, _sdkFinalImage == CapturedImage ? "RS_TakeImageData" : "Captured image", "Scoring last captured image.");
            StatusMessage = "Scored last captured image.";
        });
    }

    private async Task EncodeWsqAsync()
    {
        if (CapturedImage is null)
        {
            StatusMessage = "Capture an image before WSQ encoding.";
            return;
        }

        await RunBusyAsync(async () =>
        {
            var result = await _wsqEncodingService.EncodeAsync(CapturedImage, CancellationToken.None);
            if (!result.Success)
            {
                StatusMessage = string.IsNullOrWhiteSpace(result.ErrorMessage)
                    ? "WSQ encoding failed."
                    : result.ErrorMessage;
                return;
            }

            WsqByteLength = $"{result.WsqBytes.Length:N0} bytes";
            Array.Clear(result.WsqBytes);
            StatusMessage = "WSQ encoding completed.";
        });
    }

    private async Task AddQualityDebugResultsAsync(FingerprintImage image, string source, string notes)
    {
        var qualityMode = await _scanner.SetQualityModeAsync(2, CancellationToken.None);
        AddQualityDebugResult(new RealScanQualityDiagnosticResult(
            "RS_SetParam quality mode",
            "RS_SetParam",
            0,
            0,
            0,
            0,
            0,
            0,
            qualityMode.SdkResult,
            qualityMode.RequestedMode,
            qualityMode.SdkResult == 0 ? "OK" : "SDK Error",
            $"RS_SetParam(deviceHandle, RS_GVAR_FP_QUALITY_MODE, ref iq), iq={qualityMode.RequestedMode}.",
            qualityMode.Message));

        AddQualityDebugResult(await _qualityService.ScoreCapturedImageAsync(image, source, CancellationToken.None));
        AddQualityDebugResult(_qualityService.ScoreInverted(image, "Inverted buffer", "Derived from last captured raw 8-bit grayscale buffer."));
        AddQualityDebugResult(_qualityService.ScoreCropped(image, "Cropped buffer", "Cropped by in-memory bounding box; no WPF stride/padding bytes."));

        if (_scanner.LastPreviewFrame is not null)
        {
            var frame = _scanner.LastPreviewFrame;
            AddQualityDebugResult(_qualityService.ScoreDebugBuffer(
                "Preview frame buffer",
                "Preview callback",
                frame.Pixels,
                frame.Width,
                frame.Height,
                "Last preview callback frame."));
        }

        var current = await _scanner.TakeCurrentImageDataAsync(CancellationToken.None);
        if (current.Success)
        {
            AddQualityDebugResult(_qualityService.ScoreDebugBuffer(
                "SDK current image buffer",
                current.Source,
                current.Pixels,
                current.Width,
                current.Height,
                "Captured with RS_TakeCurrentImageData."));
            Array.Clear(current.Pixels);
        }
        else
        {
            AddQualityDebugResult(new RealScanQualityDiagnosticResult(
                "SDK current image buffer",
                current.Source,
                current.Width,
                current.Height,
                current.Pixels.Length,
                0,
                0,
                0,
                current.SdkResult,
                0,
                "Capture Error",
                current.Message,
                current.Message));
        }
    }

    private void AddQualityDebugResult(RealScanQualityDiagnosticResult result)
    {
        QualityDiagnosticResults.Add(result);
        ClearQualityDebugResultsCommand.NotifyCanExecuteChanged();
    }

    private void ClearQualityDebugResults()
    {
        QualityDiagnosticResults.Clear();
        ClearQualityDebugResultsCommand.NotifyCanExecuteChanged();
        StatusMessage = "Quality debug results cleared.";
    }

    private async Task DisconnectAsync()
    {
        await RunBusyAsync(async () =>
        {
            StopPreview();
            await _scanner.DisconnectAsync();
            ConnectedDeviceInfo = null;
            PreviewImage = null;
            StatusMessage = "Scanner disconnected.";
            OnPropertyChanged(nameof(IsConnected));
        });
    }

    private async Task RunBusyAsync(Func<Task> action)
    {
        try
        {
            IsBusy = true;
            await action();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsConnected));
        }
    }

    private void RefreshCommands()
    {
        InitializeSdkCommand.NotifyCanExecuteChanged();
        EnumerateDevicesCommand.NotifyCanExecuteChanged();
        ConnectScannerCommand.NotifyCanExecuteChanged();
        StartPreviewCommand.NotifyCanExecuteChanged();
        StopPreviewCommand.NotifyCanExecuteChanged();
        CaptureTestFingerCommand.NotifyCanExecuteChanged();
        RunQualityCommand.NotifyCanExecuteChanged();
        RunQualityDiagnosticsCommand.NotifyCanExecuteChanged();
        CaptureSdkFinalImageCommand.NotifyCanExecuteChanged();
        ScoreLastCapturedImageCommand.NotifyCanExecuteChanged();
        ClearQualityDebugResultsCommand.NotifyCanExecuteChanged();
        EncodeWsqCommand.NotifyCanExecuteChanged();
        DisconnectCommand.NotifyCanExecuteChanged();
    }

    private static BitmapSource CreateBitmap(byte[] pixels, int width, int height)
    {
        var bitmap = BitmapSource.Create(
            width,
            height,
            96,
            96,
            PixelFormats.Gray8,
            null,
            pixels,
            width);
        bitmap.Freeze();
        return bitmap;
    }
}
