using System.Runtime.InteropServices;
using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;
using RealScan;

namespace EFP_forensic.Services.Forensics;

public sealed class RealScanWsqEncodingService : IWsqEncodingService
{
    public Task<WsqEncodingResult> EncodeAsync(FingerprintImage image, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            IntPtr imageBuffer = IntPtr.Zero;
            try
            {
                imageBuffer = Marshal.AllocHGlobal(image.Pixels.Length);
                Marshal.Copy(image.Pixels, 0, imageBuffer, image.Pixels.Length);

                var wsqBufferLength = Math.Max(image.Width * image.Height * 2, 1024);
                var wsqBuffer = new byte[wsqBufferLength];
                var result = RealScanSDK.RS_EncodeWSQ(imageBuffer, image.Width, image.Height, 0.75f, wsqBuffer, ref wsqBufferLength);
                if (result != RealScanSDK.RS_SUCCESS)
                {
                    return new WsqEncodingResult
                    {
                        Success = false,
                        ErrorMessage = GetErrorMessage(result)
                    };
                }

                var encoded = new byte[wsqBufferLength];
                Buffer.BlockCopy(wsqBuffer, 0, encoded, 0, wsqBufferLength);
                return new WsqEncodingResult
                {
                    Success = true,
                    WsqBytes = encoded
                };
            }
            finally
            {
                if (imageBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(imageBuffer);
                }
            }
        }, cancellationToken);
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

        return string.IsNullOrWhiteSpace(message) ? $"RealScan WSQ error {errorCode}." : message.Trim('\0', ' ', '\r', '\n');
    }
}
