using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EFP_forensic.Domain.Enums;
using EFP_forensic.Helpers;
using EFP_forensic.Models;
using EFP_forensic.Services.Forensics;
using EFP_forensic.Services.Interfaces;
using EFP_forensic.Services.Scanner;

namespace EFP_forensic.ViewModels;

public sealed class FingerprintCaptureViewModel : ObservableObject
{
    private readonly IFingerprintScanner _scanner;
    private readonly IFingerprintQualityService _qualityService;
    private readonly IWsqEncodingService _wsqEncodingService;
    private readonly ISecureBufferCleanupService _cleanupService;
    private readonly IRealScanScannerDiagnostics? _realScanDiagnostics;
    private readonly Func<bool> _canCapture;
    private readonly Action<CapturedFingerprint> _fingerprintAccepted;
    private readonly Action<MissingFingerprint> _fingerMarkedMissing;
    private CancellationTokenSource? _previewCancellation;
    private FingerCaptureItemViewModel? _selectedFinger;
    private RealScanDeviceSummary? _selectedDevice;
    private CaptureModeOption _selectedCaptureModeOption;
    private SlapCaptureGroup _currentSlapGroup = SlapCaptureGroup.RightFour;
    private ImageSource? _previewImage;
    private string _scannerStatusMessage = "Scanner not connected.";
    private bool _isPreviewing;
    private bool _isCapturing;
    private bool _isConnectingScanner;
    private bool _showDeviceList;
    private string _statusMessage = "Confirm applicant identity to enable capture controls.";
    private int? _lastQualityScore;
    private bool _lastCaptureAccepted;
    private string _missingReason = "Unavailable";
    private RealScanQualityDiagnosticResult? _lastQualityDebugResult;
    private string _capturePathWarning = string.Empty;

    public FingerprintCaptureViewModel(
        IFingerprintScanner scanner,
        IFingerprintQualityService qualityService,
        IWsqEncodingService wsqEncodingService,
        ISecureBufferCleanupService cleanupService,
        Func<bool> canCapture,
        Action<CapturedFingerprint> fingerprintAccepted,
        Action<MissingFingerprint> fingerMarkedMissing)
    {
        _scanner = scanner;
        _qualityService = qualityService;
        _wsqEncodingService = wsqEncodingService;
        _cleanupService = cleanupService;
        _realScanDiagnostics = scanner as IRealScanScannerDiagnostics;
        _canCapture = canCapture;
        _fingerprintAccepted = fingerprintAccepted;
        _fingerMarkedMissing = fingerMarkedMissing;

        Fingers = new ObservableCollection<FingerCaptureItemViewModel>(
            Enum.GetValues<FingerPosition>().Select(finger => new FingerCaptureItemViewModel(finger)));
        Devices = [];
        CaptureModeOptions =
        [
            new CaptureModeOption(CaptureMode.Slap442, "4-4-2"),
            new CaptureModeOption(CaptureMode.Flat, "Flat"),
            new CaptureModeOption(CaptureMode.Rolled, "Roll")
        ];
        _selectedCaptureModeOption = CaptureModeOptions[0];
        MissingReasons = ["Amputated", "Injured", "Bandaged", "Unavailable", "Other"];

        InitializeScannerCommand = new AsyncRelayCommand(InitializeScannerAsync, () => !IsConnectingScanner);
        RefreshDevicesCommand = new AsyncRelayCommand(RefreshDevicesAsync, () => !IsConnectingScanner);
        ConnectSelectedDeviceCommand = new AsyncRelayCommand(ConnectSelectedDeviceAsync, () => !IsConnectingScanner && SelectedDevice is not null);
        StartPreviewCommand = new AsyncRelayCommand(StartPreviewAsync, () => CanUseCaptureControls && IsScannerConnected && !IsPreviewing);
        StopPreviewCommand = new RelayCommand(StopPreview, () => IsPreviewing);
        CaptureCommand = new AsyncRelayCommand(CaptureAsync, () => CanUseCaptureControls && IsScannerConnected && !IsCapturing && SelectedFinger is not null);
        NextFingerCommand = new RelayCommand(SelectNextFinger, () => Fingers.Count > 0);
        MarkMissingCommand = new RelayCommand(MarkSelectedFingerMissing, () => SelectedFinger is not null && SelectedFinger.Status != FingerCaptureStatus.Captured);
        RetakeFlatCommand = new ParameterizedAsyncRelayCommand<FingerCaptureItemViewModel>(finger => RetakeFingerAsync(finger, CaptureMode.Flat), CanRetakeFinger);
        RetakeRollCommand = new ParameterizedAsyncRelayCommand<FingerCaptureItemViewModel>(finger => RetakeFingerAsync(finger, CaptureMode.Rolled), CanRetakeFinger);
        MarkFingerMissingCommand = new ParameterizedRelayCommand<FingerCaptureItemViewModel>(MarkFingerMissing, finger => finger is not null);
        RetakeRightFourCommand = new AsyncRelayCommand(() => CaptureSlapGroupAsync(SlapCaptureGroup.RightFour), () => CanUseCaptureControls && IsScannerConnected && !IsCapturing);
        RetakeLeftFourCommand = new AsyncRelayCommand(() => CaptureSlapGroupAsync(SlapCaptureGroup.LeftFour), () => CanUseCaptureControls && IsScannerConnected && !IsCapturing);
        RetakeThumbsCommand = new AsyncRelayCommand(() => CaptureSlapGroupAsync(SlapCaptureGroup.Thumbs), () => CanUseCaptureControls && IsScannerConnected && !IsCapturing);

        SelectedFinger = Fingers.First();
    }

    public ObservableCollection<FingerCaptureItemViewModel> Fingers { get; }

    public ObservableCollection<RealScanDeviceSummary> Devices { get; }

    public IReadOnlyList<CaptureModeOption> CaptureModeOptions { get; }

    public IReadOnlyList<string> MissingReasons { get; }

    public FingerCaptureItemViewModel? SelectedFinger
    {
        get => _selectedFinger;
        set
        {
            if (_selectedFinger is not null)
            {
                _selectedFinger.IsCurrent = false;
            }

            if (SetProperty(ref _selectedFinger, value) && value is not null)
            {
                value.IsCurrent = true;
                CaptureCommand?.NotifyCanExecuteChanged();
                MarkMissingCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public RealScanDeviceSummary? SelectedDevice
    {
        get => _selectedDevice;
        set
        {
            if (SetProperty(ref _selectedDevice, value))
            {
                ConnectSelectedDeviceCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public CaptureMode SelectedCaptureMode => SelectedCaptureModeOption.Mode;

    public CaptureModeOption SelectedCaptureModeOption
    {
        get => _selectedCaptureModeOption;
        set
        {
            if (SetProperty(ref _selectedCaptureModeOption, value))
            {
                OnPropertyChanged(nameof(SelectedCaptureMode));
                OnPropertyChanged(nameof(IsSlap442Selected));
                OnPropertyChanged(nameof(CaptureButtonText));
                CaptureCommand?.NotifyCanExecuteChanged();
            }
        }
    }

    public ImageSource? PreviewImage
    {
        get => _previewImage;
        private set => SetProperty(ref _previewImage, value);
    }

    public string ScannerStatusMessage
    {
        get => _scannerStatusMessage;
        private set => SetProperty(ref _scannerStatusMessage, value);
    }

    public bool IsScannerConnected => _realScanDiagnostics?.IsDeviceConnected ?? false;

    public bool IsConnectingScanner
    {
        get => _isConnectingScanner;
        private set
        {
            if (SetProperty(ref _isConnectingScanner, value))
            {
                RefreshCommandStates();
            }
        }
    }

    public bool ShowDeviceList
    {
        get => _showDeviceList;
        private set => SetProperty(ref _showDeviceList, value);
    }

    public bool IsPreviewing
    {
        get => _isPreviewing;
        private set
        {
            if (SetProperty(ref _isPreviewing, value))
            {
                RefreshCommandStates();
            }
        }
    }

    public bool IsCapturing
    {
        get => _isCapturing;
        private set
        {
            if (SetProperty(ref _isCapturing, value))
            {
                RefreshCommandStates();
            }
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        private set => SetProperty(ref _statusMessage, value);
    }

    public int? LastQualityScore
    {
        get => _lastQualityScore;
        private set
        {
            if (SetProperty(ref _lastQualityScore, value))
            {
                OnPropertyChanged(nameof(QualityDisplay));
            }
        }
    }

    public bool LastCaptureAccepted
    {
        get => _lastCaptureAccepted;
        private set => SetProperty(ref _lastCaptureAccepted, value);
    }

    public string MissingReason
    {
        get => _missingReason;
        set => SetProperty(ref _missingReason, value);
    }

    public string QualityDisplay => LastQualityScore.HasValue ? $"{LastQualityScore.Value}/100" : "No capture assessed";

    public SlapCaptureGroup CurrentSlapGroup
    {
        get => _currentSlapGroup;
        private set
        {
            if (SetProperty(ref _currentSlapGroup, value))
            {
                OnPropertyChanged(nameof(CurrentSlapGroupDisplay));
                OnPropertyChanged(nameof(CaptureButtonText));
            }
        }
    }

    public string CurrentSlapGroupDisplay => CurrentSlapGroup switch
    {
        SlapCaptureGroup.RightFour => "Right Four Fingers",
        SlapCaptureGroup.LeftFour => "Left Four Fingers",
        SlapCaptureGroup.Thumbs => "Two Thumbs",
        _ => "None"
    };

    public bool IsSlap442Selected => SelectedCaptureMode == CaptureMode.Slap442;

    public string CaptureButtonText => IsSlap442Selected ? $"Capture {CurrentSlapGroupDisplay}" : "Capture";

    public RealScanQualityDiagnosticResult? LastQualityDebugResult
    {
        get => _lastQualityDebugResult;
        private set
        {
            if (SetProperty(ref _lastQualityDebugResult, value))
            {
                OnPropertyChanged(nameof(HasQualityDebugResult));
            }
        }
    }

    public bool HasQualityDebugResult => LastQualityDebugResult is not null;

    public string CapturePathWarning
    {
        get => _capturePathWarning;
        private set
        {
            if (SetProperty(ref _capturePathWarning, value))
            {
                OnPropertyChanged(nameof(HasCapturePathWarning));
            }
        }
    }

    public bool HasCapturePathWarning => !string.IsNullOrWhiteSpace(CapturePathWarning);

    public bool CanUseCaptureControls => _canCapture();

    public AsyncRelayCommand InitializeScannerCommand { get; }

    public AsyncRelayCommand RefreshDevicesCommand { get; }

    public AsyncRelayCommand ConnectSelectedDeviceCommand { get; }

    public AsyncRelayCommand StartPreviewCommand { get; }

    public RelayCommand StopPreviewCommand { get; }

    public AsyncRelayCommand CaptureCommand { get; }

    public RelayCommand NextFingerCommand { get; }

    public RelayCommand MarkMissingCommand { get; }

    public ParameterizedAsyncRelayCommand<FingerCaptureItemViewModel> RetakeFlatCommand { get; }

    public ParameterizedAsyncRelayCommand<FingerCaptureItemViewModel> RetakeRollCommand { get; }

    public ParameterizedRelayCommand<FingerCaptureItemViewModel> MarkFingerMissingCommand { get; }

    public AsyncRelayCommand RetakeRightFourCommand { get; }

    public AsyncRelayCommand RetakeLeftFourCommand { get; }

    public AsyncRelayCommand RetakeThumbsCommand { get; }

    public async Task ActivateAsync()
    {
        if (_realScanDiagnostics is null)
        {
            ScannerStatusMessage = "Scanner service is not RealScan-capable.";
            ShowDeviceList = false;
            RefreshCommandStates();
            return;
        }

        if (!_realScanDiagnostics.IsDeviceConnected)
        {
            await InitializeScannerAsync();
        }
    }

    public async Task StopPreviewAsync()
    {
        StopPreview();
        await _scanner.DisconnectAsync();
    }

    public void RefreshCommandStates()
    {
        OnPropertyChanged(nameof(CanUseCaptureControls));
        OnPropertyChanged(nameof(IsScannerConnected));
        StartPreviewCommand.NotifyCanExecuteChanged();
        StopPreviewCommand.NotifyCanExecuteChanged();
        CaptureCommand.NotifyCanExecuteChanged();
        NextFingerCommand.NotifyCanExecuteChanged();
        InitializeScannerCommand.NotifyCanExecuteChanged();
        RefreshDevicesCommand.NotifyCanExecuteChanged();
        ConnectSelectedDeviceCommand.NotifyCanExecuteChanged();
        MarkMissingCommand.NotifyCanExecuteChanged();
        RetakeFlatCommand.NotifyCanExecuteChanged();
        RetakeRollCommand.NotifyCanExecuteChanged();
        MarkFingerMissingCommand.NotifyCanExecuteChanged();
        RetakeRightFourCommand.NotifyCanExecuteChanged();
        RetakeLeftFourCommand.NotifyCanExecuteChanged();
        RetakeThumbsCommand.NotifyCanExecuteChanged();
    }

    public void Reset()
    {
        StopPreview();
        foreach (var finger in Fingers)
        {
            finger.Reset();
        }

        SelectedFinger = Fingers.FirstOrDefault();
        PreviewImage = null;
        StatusMessage = "Confirm applicant identity to enable capture controls.";
        ScannerStatusMessage = "Scanner not connected.";
        ShowDeviceList = false;
        MissingReason = "Unavailable";
        CurrentSlapGroup = SlapCaptureGroup.RightFour;
        SelectedCaptureModeOption = CaptureModeOptions[0];
        LastQualityScore = null;
        LastCaptureAccepted = false;
        LastQualityDebugResult = null;
        CapturePathWarning = string.Empty;
        RefreshCommandStates();
    }

    private async Task InitializeScannerAsync()
    {
        if (_realScanDiagnostics is null)
        {
            ScannerStatusMessage = "RealScan scanner service is not available.";
            return;
        }

        try
        {
            IsConnectingScanner = true;
            ScannerStatusMessage = "Initializing RealScan scanner...";
            var init = await _realScanDiagnostics.InitializeSdkAsync(CancellationToken.None);
            if (!init.Success)
            {
                ScannerStatusMessage = init.Message;
                ShowDeviceList = true;
                return;
            }

            await RefreshDevicesAsync();
            if (Devices.Count == 0)
            {
                ScannerStatusMessage = "No RealScan device found. Refresh devices or connect scanner.";
                ShowDeviceList = true;
                return;
            }

            SelectedDevice = Devices.First();
            await ConnectSelectedDeviceAsync();
        }
        catch (Exception ex)
        {
            ScannerStatusMessage = $"Scanner connection failed: {ex.Message}";
            ShowDeviceList = true;
        }
        finally
        {
            IsConnectingScanner = false;
            RefreshCommandStates();
        }
    }

    private async Task RefreshDevicesAsync()
    {
        if (_realScanDiagnostics is null)
        {
            return;
        }

        Devices.Clear();
        var devices = await _realScanDiagnostics.EnumerateDevicesAsync(CancellationToken.None);
        foreach (var device in devices)
        {
            Devices.Add(device);
        }

        SelectedDevice = Devices.FirstOrDefault();
        ShowDeviceList = !_realScanDiagnostics.IsDeviceConnected || Devices.Count == 0;
        if (Devices.Count > 0 && !_realScanDiagnostics.IsDeviceConnected)
        {
            ScannerStatusMessage = $"{Devices.Count} RealScan device(s) found.";
        }
    }

    private async Task ConnectSelectedDeviceAsync()
    {
        if (_realScanDiagnostics is null || SelectedDevice is null)
        {
            ScannerStatusMessage = "Select a RealScan device before connecting.";
            return;
        }

        try
        {
            IsConnectingScanner = true;
            await _realScanDiagnostics.ConnectAsync(SelectedDevice.ListIndex, CancellationToken.None);
            var info = await _realScanDiagnostics.GetConnectedDeviceInfoAsync(CancellationToken.None);
            var model = string.IsNullOrWhiteSpace(info?.ProductName) ? SelectedDevice.DeviceName : info.ProductName;
            var serial = string.IsNullOrWhiteSpace(info?.DeviceId) ? SelectedDevice.DeviceId.ToString() : info.DeviceId;
            ScannerStatusMessage = $"Scanner connected: {model} / {serial}";
            ShowDeviceList = false;
        }
        catch (Exception ex)
        {
            ScannerStatusMessage = $"Scanner connection failed: {ex.Message}";
            ShowDeviceList = true;
        }
        finally
        {
            IsConnectingScanner = false;
            RefreshCommandStates();
        }
    }

    private async Task StartPreviewAsync()
    {
        if (!CanUseCaptureControls)
        {
            StatusMessage = "Capture controls are locked until identity confirmation is complete.";
            return;
        }

        try
        {
            _previewCancellation?.Cancel();
            _previewCancellation = new CancellationTokenSource();
            IsPreviewing = true;
            StatusMessage = "Scanner preview is running.";

            await foreach (var frame in _scanner.StreamPreviewAsync(_previewCancellation.Token))
            {
                PreviewImage = CreateBitmap(frame.Pixels, frame.Width, frame.Height);
            }
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "Scanner preview stopped.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Scanner preview could not continue: {ex.Message}";
        }
        finally
        {
            IsPreviewing = false;
        }
    }

    private void StopPreview()
    {
        if (_previewCancellation is null)
        {
            return;
        }

        _previewCancellation.Cancel();
        _previewCancellation.Dispose();
        _previewCancellation = null;
        IsPreviewing = false;
        StatusMessage = "Scanner preview stopped.";
    }

    private async Task CaptureAsync()
    {
        if (SelectedCaptureMode == CaptureMode.Slap442)
        {
            await CaptureSlapGroupAsync(CurrentSlapGroup);
            return;
        }

        if (SelectedFinger is null)
        {
            StatusMessage = "Select a finger before capture.";
            return;
        }

        if (!CanUseCaptureControls)
        {
            StatusMessage = "Capture controls are locked until identity confirmation is complete.";
            return;
        }

        FingerprintImage? image = null;

        try
        {
            IsCapturing = true;
            LastCaptureAccepted = false;
            LastQualityDebugResult = null;
            CapturePathWarning = string.Empty;
            StatusMessage = $"Capturing {SelectedFinger.DisplayName}...";

            var capture = await _scanner.CaptureAsync(SelectedFinger.FingerPosition, SelectedCaptureMode, CancellationToken.None);
            if (!capture.Success || capture.Image is null)
            {
                HandleScannerDisconnectedIfNeeded(capture.ErrorMessage);
                StatusMessage = string.IsNullOrWhiteSpace(capture.ErrorMessage)
                    ? "Fingerprint capture failed."
                    : capture.ErrorMessage;
                return;
            }

            image = capture.Image;
            PreviewImage = CreateBitmap(image.Pixels, image.Width, image.Height);
            var averagePixel = CalculateAveragePixelValue(image.Pixels);

            var quality = await ScoreCapturedImageAsync(image);
            LastQualityScore = quality.Score;
            var brightnessWarning = averagePixel > 230
                ? " Image is too light. Improve finger contact."
                : string.Empty;

            if (!quality.Passed)
            {
                SelectedFinger.MarkRetryRequired(quality.Score, "Poor Ridge Definition - Please re-roll finger.", averagePixel);
                StatusMessage = "Poor Ridge Definition - Please re-roll finger.";
                LastCaptureAccepted = false;
                return;
            }

            var accepted = await BuildCapturedFingerprintAsync(image, quality, CaptureSourceForMode(SelectedCaptureMode), SlapCaptureGroup.None);
            if (accepted is null)
            {
                StatusMessage = "WSQ encoding failed.";
                return;
            }

            SelectedFinger.CommitCapture(accepted, CreateBitmap(image.Pixels, image.Width, image.Height));
            _fingerprintAccepted(accepted);
            LastCaptureAccepted = true;
            StatusMessage = $"{SelectedFinger.DisplayName} accepted. {quality.Message}.{brightnessWarning}";
            SelectNextUncapturedFinger();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Capture failed while using the scanner: {ex.Message}";
            LastCaptureAccepted = false;
        }
        finally
        {
            if (image is not null)
            {
                _cleanupService.ClearFingerprintImage(image);
            }

            IsCapturing = false;
        }
    }

    private async Task CaptureSlapGroupAsync(SlapCaptureGroup group)
    {
        if (_realScanDiagnostics is null)
        {
            StatusMessage = "RealScan slap capture is not available.";
            return;
        }

        if (!CanUseCaptureControls)
        {
            StatusMessage = "Capture controls are locked until identity confirmation is complete.";
            return;
        }

        var capturedImages = new List<FingerprintImage>();

        try
        {
            IsCapturing = true;
            LastCaptureAccepted = false;
            LastQualityDebugResult = null;
            CapturePathWarning = string.Empty;
            StatusMessage = $"Capturing {DisplaySlapGroup(group)}...";

            var slap = await _realScanDiagnostics.CaptureSlapGroupAsync(group, CancellationToken.None);
            if (!slap.Success)
            {
                HandleScannerDisconnectedIfNeeded(slap.ErrorMessage);
                StatusMessage = string.IsNullOrWhiteSpace(slap.ErrorMessage)
                    ? $"Could not capture {DisplaySlapGroup(group)}."
                    : slap.ErrorMessage;
                return;
            }

            var acceptedResults = new List<(FingerCaptureItemViewModel Finger, CapturedFingerprint Captured, ImageSource Preview)>();
            var failedResults = new List<(FingerCaptureItemViewModel Finger, int Score, string Message, double Average, ImageSource Preview)>();

            foreach (var segment in slap.Segments)
            {
                var image = segment.Image;
                capturedImages.Add(image);
                var finger = Fingers.First(item => item.FingerPosition == segment.FingerPosition);
                var preview = CreateBitmap(image.Pixels, image.Width, image.Height);
                var averagePixel = CalculateAveragePixelValue(image.Pixels);
                var quality = await ScoreCapturedImageAsync(image);

                if (!quality.Passed)
                {
                    var message = averagePixel > 230
                        ? "Poor Ridge Definition - Please re-roll finger. Image is too light. Improve finger contact."
                        : "Poor Ridge Definition - Please re-roll finger.";
                    failedResults.Add((finger, quality.Score, message, averagePixel, preview));
                    continue;
                }

                var captured = await BuildCapturedFingerprintAsync(image, quality, CaptureSource.Slap442Segment, group);
                if (captured is null)
                {
                    failedResults.Add((finger, quality.Score, "WSQ encoding failed.", averagePixel, preview));
                    continue;
                }

                acceptedResults.Add((finger, captured, preview));
            }

            foreach (var accepted in acceptedResults)
            {
                accepted.Finger.CommitCapture(accepted.Captured, accepted.Preview);
                _fingerprintAccepted(accepted.Captured);
            }

            foreach (var failed in failedResults)
            {
                failed.Finger.MarkRetryRequired(failed.Score, failed.Message, failed.Average, failed.Preview);
            }

            LastCaptureAccepted = acceptedResults.Count > 0 && failedResults.Count == 0;
            StatusMessage = failedResults.Count == 0
                ? $"{DisplaySlapGroup(group)} accepted."
                : $"{DisplaySlapGroup(group)} captured with {failedResults.Count} finger(s) requiring retry.";
            AdvanceSlapGroup(group);
            SelectNextUncapturedFinger();
        }
        catch (Exception ex)
        {
            HandleScannerDisconnectedIfNeeded(ex.Message);
            StatusMessage = $"Slap capture failed: {ex.Message}";
            LastCaptureAccepted = false;
        }
        finally
        {
            foreach (var image in capturedImages)
            {
                _cleanupService.ClearFingerprintImage(image);
            }

            IsCapturing = false;
        }
    }

    private async Task RetakeFingerAsync(FingerCaptureItemViewModel? finger, CaptureMode mode)
    {
        if (finger is null)
        {
            return;
        }

        SelectedFinger = finger;
        var previousMode = SelectedCaptureModeOption;
        SelectedCaptureModeOption = CaptureModeOptions.First(option => option.Mode == mode);
        await CaptureAsync();
        SelectedCaptureModeOption = previousMode;
    }

    private async Task<FingerprintQualityResult> ScoreCapturedImageAsync(FingerprintImage image)
    {
        CapturePathWarning = string.Empty;

        if (_qualityService is not RealScanQualityService realScanQualityService)
        {
            LastQualityDebugResult = null;
            return await _qualityService.AssessAsync(image, CancellationToken.None);
        }

        var source = string.IsNullOrWhiteSpace(image.Source) ? "RS_TakeImageData" : image.Source;
        var diagnostic = await realScanQualityService.ScoreCapturedImageAsync(image, source, CancellationToken.None);
        LastQualityDebugResult = diagnostic;

        var comparison = await realScanQualityService.ScoreCapturedImageAsync(image, "Diagnostics comparison on same raw buffer", CancellationToken.None);
        if (comparison.QualityScore != diagnostic.QualityScore ||
            comparison.SdkResult != diagnostic.SdkResult ||
            comparison.Width != diagnostic.Width ||
            comparison.Height != diagnostic.Height ||
            comparison.ByteLength != diagnostic.ByteLength)
        {
            CapturePathWarning = "Main capture path differs from diagnostics path.";
        }

        if (diagnostic.SdkResult != 0)
        {
            return new FingerprintQualityResult
            {
                Score = 0,
                Passed = false,
                Message = diagnostic.Message
            };
        }

        return new FingerprintQualityResult
        {
            Score = diagnostic.QualityScore,
            Passed = diagnostic.QualityScore >= 60,
            Message = diagnostic.QualityScore switch
            {
                < 60 => "Poor Ridge Definition",
                < 80 => "Acceptable Quality",
                _ => "Excellent Quality"
            }
        };
    }

    private async Task<CapturedFingerprint?> BuildCapturedFingerprintAsync(
        FingerprintImage image,
        FingerprintQualityResult quality,
        CaptureSource captureSource,
        SlapCaptureGroup parentSlapGroup)
    {
        var encoded = await _wsqEncodingService.EncodeAsync(image, CancellationToken.None);
        if (!encoded.Success)
        {
            return null;
        }

        return new CapturedFingerprint
        {
            FingerPosition = image.FingerPosition,
            CaptureMode = image.CaptureMode,
            CaptureSource = captureSource,
            ParentSlapGroup = parentSlapGroup,
            QualityScore = quality.Score,
            QualityLabel = quality.Message,
            WsqBytes = encoded.WsqBytes,
            Width = image.Width,
            Height = image.Height,
            Dpi = image.Dpi,
            AveragePixelValue = CalculateAveragePixelValue(image.Pixels),
            CapturedAt = image.CapturedAt
        };
    }

    private static CaptureSource CaptureSourceForMode(CaptureMode mode)
    {
        return mode switch
        {
            CaptureMode.Rolled => CaptureSource.Roll,
            CaptureMode.Flat => CaptureSource.Flat,
            _ => CaptureSource.Slap442Segment
        };
    }

    private void AdvanceSlapGroup(SlapCaptureGroup capturedGroup)
    {
        CurrentSlapGroup = capturedGroup switch
        {
            SlapCaptureGroup.RightFour => SlapCaptureGroup.LeftFour,
            SlapCaptureGroup.LeftFour => SlapCaptureGroup.Thumbs,
            SlapCaptureGroup.Thumbs => SlapCaptureGroup.RightFour,
            _ => SlapCaptureGroup.RightFour
        };
    }

    private static string DisplaySlapGroup(SlapCaptureGroup group)
    {
        return group switch
        {
            SlapCaptureGroup.RightFour => "Right Four Fingers",
            SlapCaptureGroup.LeftFour => "Left Four Fingers",
            SlapCaptureGroup.Thumbs => "Two Thumbs",
            _ => "Slap Capture"
        };
    }

    private bool CanRetakeFinger(FingerCaptureItemViewModel? finger)
    {
        return finger is not null && CanUseCaptureControls && IsScannerConnected && !IsCapturing;
    }

    private void MarkFingerMissing(FingerCaptureItemViewModel? finger)
    {
        if (finger is null)
        {
            return;
        }

        SelectedFinger = finger;
        MarkSelectedFingerMissing();
    }

    private void HandleScannerDisconnectedIfNeeded(string message)
    {
        if (_realScanDiagnostics?.IsDeviceConnected == false || IsDisconnectMessage(message))
        {
            StopPreview();
            ScannerStatusMessage = "Scanner disconnected. Please reconnect the device.";
            ShowDeviceList = true;
            RefreshCommandStates();
        }
    }

    private static bool IsDisconnectMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return false;
        }

        return message.Contains("no device", StringComparison.OrdinalIgnoreCase)
            || message.Contains("usb", StringComparison.OrdinalIgnoreCase)
            || message.Contains("disconnect", StringComparison.OrdinalIgnoreCase)
            || message.Contains("not initialized", StringComparison.OrdinalIgnoreCase)
            || message.Contains("invalid device connection", StringComparison.OrdinalIgnoreCase);
    }

    private void SelectNextFinger()
    {
        if (SelectedFinger is null)
        {
            SelectedFinger = Fingers.FirstOrDefault();
            return;
        }

        var nextIndex = (Fingers.IndexOf(SelectedFinger) + 1) % Fingers.Count;
        SelectedFinger = Fingers[nextIndex];
    }

    private void SelectNextUncapturedFinger()
    {
        var next = Fingers.FirstOrDefault(finger => finger.Status is not FingerCaptureStatus.Captured and not FingerCaptureStatus.Missing);
        if (next is not null)
        {
            SelectedFinger = next;
        }
    }

    private void MarkSelectedFingerMissing()
    {
        if (SelectedFinger is null)
        {
            return;
        }

        var confirm = System.Windows.MessageBox.Show(
            "Mark this finger as missing? This will be submitted as a missing finger record.",
            "Confirm Missing Finger",
            System.Windows.MessageBoxButton.YesNo,
            System.Windows.MessageBoxImage.Warning);

        if (confirm != System.Windows.MessageBoxResult.Yes)
        {
            return;
        }

        SelectedFinger.MarkMissing(MissingReason);
        _fingerMarkedMissing(new MissingFingerprint
        {
            FingerPosition = SelectedFinger.FingerPosition,
            IsMissing = true,
            MissingReason = MissingReason,
            MarkedMissingAt = SelectedFinger.MarkedMissingAt ?? DateTimeOffset.UtcNow
        });
        StatusMessage = $"{SelectedFinger.DisplayName} marked missing.";
        SelectNextUncapturedFinger();
    }

    private static double CalculateAveragePixelValue(byte[] pixels)
    {
        if (pixels.Length == 0)
        {
            return 0;
        }

        long sum = 0;
        foreach (var pixel in pixels)
        {
            sum += pixel;
        }

        return (double)sum / pixels.Length;
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
