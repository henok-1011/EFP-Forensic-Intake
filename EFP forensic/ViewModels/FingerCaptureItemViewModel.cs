using EFP_forensic.Domain.Enums;
using EFP_forensic.Helpers;
using EFP_forensic.Models;
using System.Windows.Media;

namespace EFP_forensic.ViewModels;

public sealed class FingerCaptureItemViewModel : ObservableObject
{
    private FingerCaptureStatus _status = FingerCaptureStatus.Pending;
    private bool _isCurrent;
    private int? _qualityScore;
    private string _qualityLabel = string.Empty;
    private double? _averagePixelValue;
    private string _missingReason = string.Empty;
    private DateTimeOffset? _markedMissingAt;
    private string _errorMessage = string.Empty;
    private DateTimeOffset? _capturedAt;
    private CaptureSource? _captureSource;
    private SlapCaptureGroup _parentSlapGroup = SlapCaptureGroup.None;
    private byte[] _wsqBytes = [];
    private ImageSource? _imagePreview;
    private int _width;
    private int _height;
    private int _dpi;

    public FingerCaptureItemViewModel(FingerPosition fingerPosition)
    {
        FingerPosition = fingerPosition;
    }

    public FingerPosition FingerPosition { get; }

    public string DisplayName => SplitCamelCase(FingerPosition.ToString());

    public FingerCaptureStatus Status
    {
        get => _status;
        private set
        {
            if (SetProperty(ref _status, value))
            {
                OnPropertyChanged(nameof(StatusText));
                OnPropertyChanged(nameof(IsCaptured));
                OnPropertyChanged(nameof(IsMissing));
            }
        }
    }

    public bool IsCaptured => Status == FingerCaptureStatus.Captured;

    public bool IsMissing => Status == FingerCaptureStatus.Missing;

    public bool HasImagePreview => ImagePreview is not null;

    public bool IsCurrent
    {
        get => _isCurrent;
        set
        {
            if (SetProperty(ref _isCurrent, value))
            {
                OnPropertyChanged(nameof(StatusText));
            }
        }
    }

    public int? QualityScore
    {
        get => _qualityScore;
        private set
        {
            if (SetProperty(ref _qualityScore, value))
            {
                OnPropertyChanged(nameof(QualityDisplay));
            }
        }
    }

    public string QualityLabel
    {
        get => _qualityLabel;
        private set => SetProperty(ref _qualityLabel, value);
    }

    public double? AveragePixelValue
    {
        get => _averagePixelValue;
        private set
        {
            if (SetProperty(ref _averagePixelValue, value))
            {
                OnPropertyChanged(nameof(AverageDisplay));
            }
        }
    }

    public string AverageDisplay => AveragePixelValue.HasValue ? $"{AveragePixelValue.Value:F2}" : "n/a";

    public string MissingReason
    {
        get => _missingReason;
        set => SetProperty(ref _missingReason, value);
    }

    public DateTimeOffset? MarkedMissingAt
    {
        get => _markedMissingAt;
        private set => SetProperty(ref _markedMissingAt, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        private set => SetProperty(ref _errorMessage, value);
    }

    public DateTimeOffset? CapturedAt
    {
        get => _capturedAt;
        private set => SetProperty(ref _capturedAt, value);
    }

    public CaptureSource? CaptureSource
    {
        get => _captureSource;
        private set => SetProperty(ref _captureSource, value);
    }

    public SlapCaptureGroup ParentSlapGroup
    {
        get => _parentSlapGroup;
        private set => SetProperty(ref _parentSlapGroup, value);
    }

    public byte[] WsqBytes
    {
        get => _wsqBytes;
        private set => SetProperty(ref _wsqBytes, value);
    }

    public ImageSource? ImagePreview
    {
        get => _imagePreview;
        private set
        {
            if (SetProperty(ref _imagePreview, value))
            {
                OnPropertyChanged(nameof(HasImagePreview));
            }
        }
    }

    public int Width
    {
        get => _width;
        private set => SetProperty(ref _width, value);
    }

    public int Height
    {
        get => _height;
        private set => SetProperty(ref _height, value);
    }

    public int Dpi
    {
        get => _dpi;
        private set => SetProperty(ref _dpi, value);
    }

    public string QualityDisplay => QualityScore.HasValue ? $"{QualityScore.Value}/100" : StatusText;

    public string StatusText => Status switch
    {
        FingerCaptureStatus.Captured => "Captured",
        FingerCaptureStatus.RetryRequired => "Retry Required",
        FingerCaptureStatus.Missing => "Missing",
        _ => IsCurrent ? "Current" : "Pending"
    };

    public void MarkCaptured(int qualityScore, string qualityLabel, double averagePixelValue, DateTimeOffset capturedAt)
    {
        QualityScore = qualityScore;
        QualityLabel = qualityLabel;
        AveragePixelValue = averagePixelValue;
        CapturedAt = capturedAt;
        ErrorMessage = string.Empty;
        Status = FingerCaptureStatus.Captured;
    }

    public void CommitCapture(CapturedFingerprint captured, ImageSource imagePreview)
    {
        Array.Clear(WsqBytes);
        QualityScore = captured.QualityScore;
        QualityLabel = captured.QualityLabel;
        AveragePixelValue = captured.AveragePixelValue;
        CapturedAt = captured.CapturedAt;
        CaptureSource = captured.CaptureSource;
        ParentSlapGroup = captured.ParentSlapGroup;
        WsqBytes = captured.WsqBytes;
        ImagePreview = imagePreview;
        Width = captured.Width;
        Height = captured.Height;
        Dpi = captured.Dpi;
        MissingReason = string.Empty;
        MarkedMissingAt = null;
        ErrorMessage = string.Empty;
        Status = FingerCaptureStatus.Captured;
    }

    public void MarkRetryRequired(int qualityScore, string errorMessage, double averagePixelValue)
    {
        QualityScore = qualityScore;
        QualityLabel = "Poor Ridge Definition";
        AveragePixelValue = averagePixelValue;
        ErrorMessage = errorMessage;
        Status = FingerCaptureStatus.RetryRequired;
    }

    public void MarkRetryRequired(int qualityScore, string errorMessage, double averagePixelValue, ImageSource? imagePreview)
    {
        MarkRetryRequired(qualityScore, errorMessage, averagePixelValue);
        ImagePreview = imagePreview;
    }

    public void MarkMissing(string reason)
    {
        Array.Clear(WsqBytes);
        QualityScore = null;
        QualityLabel = string.Empty;
        AveragePixelValue = null;
        CaptureSource = null;
        ParentSlapGroup = SlapCaptureGroup.None;
        WsqBytes = [];
        ImagePreview = null;
        Width = 0;
        Height = 0;
        Dpi = 0;
        MissingReason = reason;
        MarkedMissingAt = DateTimeOffset.UtcNow;
        ErrorMessage = string.Empty;
        Status = FingerCaptureStatus.Missing;
    }

    public void Reset()
    {
        Array.Clear(WsqBytes);
        QualityScore = null;
        QualityLabel = string.Empty;
        AveragePixelValue = null;
        CaptureSource = null;
        ParentSlapGroup = SlapCaptureGroup.None;
        WsqBytes = [];
        ImagePreview = null;
        Width = 0;
        Height = 0;
        Dpi = 0;
        MissingReason = string.Empty;
        MarkedMissingAt = null;
        ErrorMessage = string.Empty;
        CapturedAt = null;
        Status = FingerCaptureStatus.Pending;
        IsCurrent = false;
    }

    private static string SplitCamelCase(string value)
    {
        return string.Concat(value.Select((character, index) =>
            index > 0 && char.IsUpper(character) ? $" {character}" : character.ToString()));
    }
}
