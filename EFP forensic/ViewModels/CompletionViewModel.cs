using EFP_forensic.Helpers;
using EFP_forensic.Models;

namespace EFP_forensic.ViewModels;

public sealed class CompletionViewModel : ObservableObject
{
    private readonly Action _newIntake;
    private BiometricSubmissionResult? _submissionResult;

    public CompletionViewModel(Action newIntake)
    {
        _newIntake = newIntake;
        NewIntakeCommand = new RelayCommand(_newIntake);
    }

    public BiometricSubmissionResult? SubmissionResult
    {
        get => _submissionResult;
        set => SetProperty(ref _submissionResult, value);
    }

    public RelayCommand NewIntakeCommand { get; }

    public void Reset()
    {
        SubmissionResult = null;
    }
}
