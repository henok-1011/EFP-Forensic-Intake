using System.Collections.ObjectModel;
using EFP_forensic.Helpers;
using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;

namespace EFP_forensic.ViewModels;

public sealed class SubmissionReviewViewModel : ObservableObject
{
    private readonly IBiometricSubmissionService _submissionService;
    private readonly Func<BiometricSubmissionPackage?> _createPackage;
    private readonly Action<BiometricSubmissionResult> _submissionCompleted;
    private bool _isSubmitting;
    private string _statusMessage = string.Empty;

    public SubmissionReviewViewModel(
        IBiometricSubmissionService submissionService,
        ObservableCollection<CapturedFingerprint> capturedFingerprints,
        ObservableCollection<MissingFingerprint> missingFingerprints,
        Func<BiometricSubmissionPackage?> createPackage,
        Action<BiometricSubmissionResult> submissionCompleted)
    {
        _submissionService = submissionService;
        CapturedFingerprints = capturedFingerprints;
        MissingFingerprints = missingFingerprints;
        _createPackage = createPackage;
        _submissionCompleted = submissionCompleted;
        SubmitCommand = new AsyncRelayCommand(SubmitAsync, () => !IsSubmitting && (CapturedFingerprints.Count > 0 || MissingFingerprints.Count > 0));
        CapturedFingerprints.CollectionChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(CapturedCount));
            SubmitCommand.NotifyCanExecuteChanged();
        };
        MissingFingerprints.CollectionChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(MissingCount));
            SubmitCommand.NotifyCanExecuteChanged();
        };
    }

    public ObservableCollection<CapturedFingerprint> CapturedFingerprints { get; }

    public ObservableCollection<MissingFingerprint> MissingFingerprints { get; }

    public int CapturedCount => CapturedFingerprints.Count;

    public int MissingCount => MissingFingerprints.Count;

    public bool IsSubmitting
    {
        get => _isSubmitting;
        private set
        {
            if (SetProperty(ref _isSubmitting, value))
            {
                SubmitCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        private set => SetProperty(ref _statusMessage, value);
    }

    public AsyncRelayCommand SubmitCommand { get; }

    public void Reset()
    {
        StatusMessage = string.Empty;
        IsSubmitting = false;
        SubmitCommand.NotifyCanExecuteChanged();
    }

    private async Task SubmitAsync()
    {
        var package = _createPackage();
        if (package is null)
        {
            StatusMessage = "No valid applicant is loaded for submission.";
            return;
        }

        try
        {
            IsSubmitting = true;
            StatusMessage = "Submitting biometric package...";
            var result = await _submissionService.SubmitAsync(package, CancellationToken.None);
            StatusMessage = result.Message;
            _submissionCompleted(result);
        }
        catch (Exception)
        {
            StatusMessage = "Biometric submission failed.";
        }
        finally
        {
            IsSubmitting = false;
        }
    }
}
