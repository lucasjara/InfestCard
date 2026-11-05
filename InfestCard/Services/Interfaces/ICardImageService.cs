using InfestCard.Models;

namespace InfestCard.Services.Interfaces;

public interface ICardImageService
{
    Task<Card> PrepareImageAsync(Card card, CancellationToken cancellationToken = default);
}
