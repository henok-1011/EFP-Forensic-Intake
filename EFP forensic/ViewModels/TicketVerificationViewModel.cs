using EFP_forensic.Helpers;
using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;

namespace EFP_forensic.ViewModels;

public sealed class TicketVerificationViewModel : ObservableObject
{
    private readonly IIntakeVerificationService _verificationService;
    private readonly Action<IntakeVerificationResult> _verificationCompleted;
    private string _trackingNumber = "EFP-2026-00001";
    private bool _isVerifying;
    private string _message = string.Empty;
    private bool _hasError;

    public TicketVerificationViewModel(
        IIntakeVerificationService verificationService,
        Action<IntakeVerificationResult> verificationCompleted)
    {
        _verificationService = verificationService;
        _verificationCompleted = verificationCompleted;
        VerifyCommand = new AsyncRelayCommand(VerifyAsync, () => !IsVerifying);
    }

    public string TrackingNumber
    {
        get => _trackingNumber;
        set => SetProperty(ref _trackingNumber, value);
    }

    public bool IsVerifying
    {
        get => _isVerifying;
        private set
        {
            if (SetProperty(ref _isVerifying, value))
            {
                VerifyCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public string Message
    {
        get => _message;
        private set => SetProperty(ref _message, value);
    }

    public bool HasMessage => !string.IsNullOrWhiteSpace(Message);

    public bool HasError
    {
        get => _hasError;
        private set => SetProperty(ref _hasError, value);
    }

    public AsyncRelayCommand VerifyCommand { get; }

    public void Reset()
    {
        Message = string.Empty;
        OnPropertyChanged(nameof(HasMessage));
        HasError = false;
    }

    private async Task VerifyAsync()
    {
        try
        {
            IsVerifying = true;
            HasError = false;
            Message = "Verifying intake ticket...";
            OnPropertyChanged(nameof(HasMessage));

            var result = await _verificationService.VerifyAsync(TrackingNumber, CancellationToken.None);
            HasError = !result.Success;
            Message = result.Message;
            OnPropertyChanged(nameof(HasMessage));
            _verificationCompleted(result);
        }
        catch (OperationCanceledException)
        {
            HasError = true;
            Message = "Verification was cancelled.";
            OnPropertyChanged(nameof(HasMessage));
        }
        catch (Exception)
        {
            HasError = true;
            Message = "An unexpected verification error occurred.";
            OnPropertyChanged(nameof(HasMessage));
        }
        finally
        {
            IsVerifying = false;
        }
    }
}
