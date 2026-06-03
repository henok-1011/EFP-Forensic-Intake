using EFP_forensic.Helpers;
using EFP_forensic.Models;

namespace EFP_forensic.ViewModels;

public sealed class ApplicantReviewViewModel : ObservableObject
{
    private readonly Action<bool> _identityConfirmationChanged;
    private readonly Action _continueToCapture;
    private ApplicantProfile? _applicant;
    private bool _identityConfirmed;

    public ApplicantReviewViewModel(Action<bool> identityConfirmationChanged, Action continueToCapture)
    {
        _identityConfirmationChanged = identityConfirmationChanged;
        _continueToCapture = continueToCapture;
        ContinueCommand = new RelayCommand(Continue, () => IdentityConfirmed && Applicant is not null);
    }

    public ApplicantProfile? Applicant
    {
        get => _applicant;
        set
        {
            if (SetProperty(ref _applicant, value))
            {
                ContinueCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public bool IdentityConfirmed
    {
        get => _identityConfirmed;
        set
        {
            if (SetProperty(ref _identityConfirmed, value))
            {
                _identityConfirmationChanged(value);
                ContinueCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public RelayCommand ContinueCommand { get; }

    public void Reset()
    {
        Applicant = null;
        IdentityConfirmed = false;
    }

    private void Continue()
    {
        _continueToCapture();
    }
}
