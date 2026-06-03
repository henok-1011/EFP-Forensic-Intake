using System.Windows;
using EFP_forensic.Models;
using EFP_forensic.Services.Forensics;
using EFP_forensic.Services.Interfaces;
using EFP_forensic.Services.Mock;
using EFP_forensic.Services.Scanner;
using EFP_forensic.Services.Security;
using EFP_forensic.ViewModels;

namespace EFP_forensic;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var configuration = new AppConfiguration
        {
            UseMockScanner = false
        };

        IIntakeVerificationService verificationService = new MockIntakeVerificationService();
        var realScanScanner = new RealScanSdkFingerprintScanner();
        IFingerprintScanner scanner = realScanScanner;
        IFingerprintQualityService qualityService = new RealScanQualityService(realScanScanner);
        IWsqEncodingService wsqEncodingService = new RealScanWsqEncodingService();
        IBiometricSubmissionService submissionService = new MockBiometricSubmissionService();
        ISecureBufferCleanupService cleanupService = new SecureBufferCleanupService();

        DataContext = new MainViewModel(
            configuration,
            verificationService,
            scanner,
            qualityService,
            wsqEncodingService,
            submissionService,
            cleanupService);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }
}
