using InfestCard.Models;

namespace InfestCard.Services.Interfaces;

public interface ICardStorageService
{
    Task<IReadOnlyList<Card>> GetCardsAsync(CancellationToken cancellationToken = default);

    Task<Card?> GetCardByIdAsync(string id, CancellationToken cancellationToken = default);

    Task SaveCardAsync(Card card, CancellationToken cancellationToken = default);
}
