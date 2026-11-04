using InfestCard.Services.Interfaces;

namespace InfestCard.Services.Implementations;

public sealed class QrScannerService : IQrScannerService
{
    private string? lastAcceptedCode;

    public string? AcceptCode(string? value)
    {
        var normalizedValue = value?.Trim();
        if (string.IsNullOrWhiteSpace(normalizedValue) ||
            string.Equals(normalizedValue, lastAcceptedCode, StringComparison.Ordinal))
        {
            return null;
        }

        lastAcceptedCode = normalizedValue;
        return normalizedValue;
    }
}
