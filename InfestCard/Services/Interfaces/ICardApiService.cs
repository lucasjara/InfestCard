using InfestCard.Models;

namespace InfestCard.Services.Interfaces;

public interface ICardApiService
{
    Task<Card> GetCardFromQrAsync(string qrPayload, CancellationToken cancellationToken = default);
}
