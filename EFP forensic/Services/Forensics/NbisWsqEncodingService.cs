using EFP_forensic.Models;
using EFP_forensic.Services.Interfaces;

namespace EFP_forensic.Services.Forensics;

public sealed class NbisWsqEncodingService : IWsqEncodingService
{
    public Task<WsqEncodingResult> EncodeAsync(FingerprintImage image, CancellationToken cancellationToken)
    {
        // TODO: Call an approved NBIS WSQ wrapper after native signatures and memory handling are reviewed.
        throw new NotImplementedException("NBIS WSQ encoding is not integrated.");
    }
}
