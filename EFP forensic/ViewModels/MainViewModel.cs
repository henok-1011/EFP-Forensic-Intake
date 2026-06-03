using System.Collections.ObjectModel;
using System.Windows.Threading;
using EFP_forensic.Domain.Enums;
using EFP_forensic.Helpers;
using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;
using EFP_forensic.Services.Scanner;

namespace EFP_forensic.ViewModels;

public sealed class MainViewModel : ObservableObject
{
    private readonly AppConfiguration _configuration;
    private readonly ISecureBufferCleanupService _cleanupService;
    private readonly IRealScanScannerDiagnostics? _scannerDiagnostics;
    private WorkflowStep _currentStep = WorkflowStep.TicketVerification;
    private ApplicantProfile? _applicant;
    private string _stationStatus = "Online / Scanner Mock Connected";
    private DateTimeOffset _currentTime = DateTimeOffset.Now;
    private bool _identityConfirmed;
    private BiometricSubmissionPackage? _activeSubmissionPackage;

    public MainViewModel(
        AppConfiguration configuration,
        IIntakeVerificationService verificationService,
        IFingerprintScanner scanner,
        IFingerprintQualityService qualityService,
        IWsqEncodingService wsqEncodingService,
        IBiometricSubmissionService submissionService,
        ISecureBufferCleanupService cleanupService)
    {
        _configuration = configuration;
        _cleanupService = cleanupService;
        _scannerDiagnostics = scanner as IRealScanScannerDiagnostics;
        CapturedFingerprints = [];
        MissingFingerprints = [];

        TicketVerification = new TicketVerificationViewModel(verificationService, OnVerificationCompleted);
        ApplicantReview = new ApplicantReviewViewModel(OnIdentityConfirmationChanged, GoToCapture);
        FingerprintCapture = new FingerprintCaptureViewModel(
            scanner,
            qualityService,
            wsqEncodingService,
            cleanupService,
            CanCapture,
            OnFingerprintAccepted,
            OnFingerMarkedMissing);
        SubmissionReview = new SubmissionReviewViewModel(
            submissionService,
            CapturedFingerprints,
            MissingFingerprints,
            CreateSubmissionPackage,
            OnSubmissionCompleted);
        Completion = new CompletionViewModel(ResetSession);

        GoToSubmissionCommand = new RelayCommand(GoToSubmission, () => CanSubmit);
        BackToCaptureCommand = new RelayCommand(() => CurrentStep = WorkflowStep.FingerprintCapture);
        NewIntakeCommand = new RelayCommand(ResetSession);

        var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        timer.Tick += (_, _) => CurrentTime = DateTimeOffset.Now;
        timer.Start();
    }

    public TicketVerificationViewModel TicketVerification { get; }

    public ApplicantReviewViewModel ApplicantReview { get; }

    public FingerprintCaptureViewModel FingerprintCapture { get; }

    public SubmissionReviewViewModel SubmissionReview { get; }

    public CompletionViewModel Completion { get; }

    public ObservableCollection<CapturedFingerprint> CapturedFingerprints { get; }

    public ObservableCollection<MissingFingerprint> MissingFingerprints { get; }

    public AppConfiguration Configuration => _configuration;

    public WorkflowStep CurrentStep
    {
        get => _currentStep;
        private set
        {
            if (SetProperty(ref _currentStep, value))
            {
                OnPropertyChanged(nameof(IsTicketVerificationVisible));
                OnPropertyChanged(nameof(IsApplicantReviewVisible));
                OnPropertyChanged(nameof(IsFingerprintCaptureVisible));
                OnPropertyChanged(nameof(IsSubmissionReviewVisible));
                OnPropertyChanged(nameof(IsCompletionVisible));
                OnPropertyChanged(nameof(CurrentStepName));
            }
        }
    }

    public ApplicantProfile? Applicant
    {
        get => _applicant;
        private set
        {
            if (SetProperty(ref _applicant, value))
            {
                OnPropertyChanged(nameof(ApplicantName));
                OnPropertyChanged(nameof(TrackingNumber));
                OnPropertyChanged(nameof(CanSubmit));
            }
        }
    }

    public string StationStatus
    {
        get => _stationStatus;
        private set => SetProperty(ref _stationStatus, value);
    }

    public DateTimeOffset CurrentTime
    {
        get => _currentTime;
        private set => SetProperty(ref _currentTime, value);
    }

    public bool IdentityConfirmed
    {
        get => _identityConfirmed;
        private set
        {
            if (SetProperty(ref _identityConfirmed, value))
            {
                FingerprintCapture.RefreshCommandStates();
                OnPropertyChanged(nameof(CanSubmit));
            }
        }
    }

    public string OfficerId => _configuration.OfficerId;

    public string StationId => _configuration.StationId;

    public string ApplicantName => Applicant?.FullName ?? "No applicant loaded";

    public string TrackingNumber => Applicant?.TrackingNumber ?? "None";

    public int CapturedCount => CapturedFingerprints.Count;

    public bool CanSubmit => Applicant is
    {
        PaymentStatus: PaymentStatus.Paid,
        IntakeStatus: IntakeStatus.PendingIntake
    } && (CapturedFingerprints.Count > 0 || MissingFingerprints.Count > 0);

    public string CurrentStepName => CurrentStep switch
    {
        WorkflowStep.TicketVerification => "Ticket Verification",
        WorkflowStep.ApplicantReview => "Applicant Review",
        WorkflowStep.FingerprintCapture => "Fingerprint Capture",
        WorkflowStep.SubmissionReview => "Submission",
        WorkflowStep.Completion => "Completion",
        _ => "Session"
    };

    public bool IsTicketVerificationVisible => CurrentStep == WorkflowStep.TicketVerification;

    public bool IsApplicantReviewVisible => CurrentStep == WorkflowStep.ApplicantReview;

    public bool IsFingerprintCaptureVisible => CurrentStep == WorkflowStep.FingerprintCapture;

    public bool IsSubmissionReviewVisible => CurrentStep == WorkflowStep.SubmissionReview;

    public bool IsCompletionVisible => CurrentStep == WorkflowStep.Completion;

    public RelayCommand GoToSubmissionCommand { get; }

    public RelayCommand BackToCaptureCommand { get; }

    public RelayCommand NewIntakeCommand { get; }

    private void OnVerificationCompleted(IntakeVerificationResult result)
    {
        if (!result.Success || result.ApplicantProfile is null)
        {
            return;
        }

        Applicant = result.ApplicantProfile;
        ApplicantReview.Applicant = result.ApplicantProfile;
        IdentityConfirmed = false;
        CurrentStep = WorkflowStep.ApplicantReview;
    }

    private void OnIdentityConfirmationChanged(bool confirmed)
    {
        IdentityConfirmed = confirmed;
    }

    private async void GoToCapture()
    {
        if (Applicant is null)
        {
            return;
        }

        CurrentStep = WorkflowStep.FingerprintCapture;
        await FingerprintCapture.ActivateAsync();
    }

    private bool CanCapture()
    {
        return IdentityConfirmed
               && Applicant?.PaymentStatus == PaymentStatus.Paid
               && Applicant.IntakeStatus == IntakeStatus.PendingIntake;
    }

    private void OnFingerprintAccepted(CapturedFingerprint captured)
    {
        var existing = CapturedFingerprints.FirstOrDefault(item => item.FingerPosition == captured.FingerPosition);
        if (existing is not null)
        {
            Array.Clear(existing.WsqBytes);
            CapturedFingerprints.Remove(existing);
        }

        var missing = MissingFingerprints.FirstOrDefault(item => item.FingerPosition == captured.FingerPosition);
        if (missing is not null)
        {
            MissingFingerprints.Remove(missing);
        }

        CapturedFingerprints.Add(captured);
        OnPropertyChanged(nameof(CapturedCount));
        OnPropertyChanged(nameof(CanSubmit));
        GoToSubmissionCommand.NotifyCanExecuteChanged();
        SubmissionReview.SubmitCommand.NotifyCanExecuteChanged();
    }

    private void OnFingerMarkedMissing(MissingFingerprint missing)
    {
        var existingCapture = CapturedFingerprints.FirstOrDefault(item => item.FingerPosition == missing.FingerPosition);
        if (existingCapture is not null)
        {
            Array.Clear(existingCapture.WsqBytes);
            CapturedFingerprints.Remove(existingCapture);
        }

        var existingMissing = MissingFingerprints.FirstOrDefault(item => item.FingerPosition == missing.FingerPosition);
        if (existingMissing is not null)
        {
            MissingFingerprints.Remove(existingMissing);
        }

        MissingFingerprints.Add(missing);
        OnPropertyChanged(nameof(CapturedCount));
        OnPropertyChanged(nameof(CanSubmit));
        GoToSubmissionCommand.NotifyCanExecuteChanged();
        SubmissionReview.SubmitCommand.NotifyCanExecuteChanged();
    }

    private void GoToSubmission()
    {
        if (!CanSubmit)
        {
            return;
        }

        CurrentStep = WorkflowStep.SubmissionReview;
    }

    private BiometricSubmissionPackage? CreateSubmissionPackage()
    {
        if (Applicant is null)
        {
            return null;
        }

        if (Applicant.PaymentStatus != PaymentStatus.Paid || Applicant.IntakeStatus != IntakeStatus.PendingIntake)
        {
            return null;
        }

        var scannerInfo = _scannerDiagnostics?.ConnectedDevice;

        _activeSubmissionPackage = new BiometricSubmissionPackage
        {
            TrackingNumber = Applicant.TrackingNumber,
            ApplicantFullName = Applicant.FullName,
            Fingerprints = CapturedFingerprints.ToList(),
            MissingFingerprints = MissingFingerprints.ToList(),
            StationId = _configuration.StationId,
            OfficerId = _configuration.OfficerId,
            ScannerModel = scannerInfo?.DeviceName ?? string.Empty,
            ScannerSerial = scannerInfo?.DeviceId.ToString() ?? string.Empty,
            SubmittedAt = DateTimeOffset.UtcNow
        };

        return _activeSubmissionPackage;
    }

    private void OnSubmissionCompleted(BiometricSubmissionResult result)
    {
        if (!result.Success)
        {
            return;
        }

        Completion.SubmissionResult = result;
        CurrentStep = WorkflowStep.Completion;
    }

    private async void ResetSession()
    {
        await FingerprintCapture.StopPreviewAsync();

        if (_activeSubmissionPackage is not null)
        {
            _cleanupService.ClearSubmissionPackage(_activeSubmissionPackage);
            _activeSubmissionPackage = null;
        }

        foreach (var captured in CapturedFingerprints)
        {
            Array.Clear(captured.WsqBytes);
        }

        CapturedFingerprints.Clear();
        MissingFingerprints.Clear();
        Applicant = null;
        IdentityConfirmed = false;
        TicketVerification.Reset();
        ApplicantReview.Reset();
        FingerprintCapture.Reset();
        SubmissionReview.Reset();
        Completion.Reset();
        StationStatus = _configuration.UseMockScanner ? "Online / Scanner Mock Connected" : "Online";
        CurrentStep = WorkflowStep.TicketVerification;
        OnPropertyChanged(nameof(CapturedCount));
        OnPropertyChanged(nameof(CanSubmit));
        GoToSubmissionCommand.NotifyCanExecuteChanged();
    }
}
