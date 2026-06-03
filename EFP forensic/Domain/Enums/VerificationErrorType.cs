namespace EFP_forensic.Domain.Enums;

public enum VerificationErrorType
{
    None,
    NotFound,
    PaymentPending,
    AlreadyProcessed,
    NetworkError,
    ServerError,
    Unknown
}
