using InfestCard.Data.Entities;
using InfestCard.Data.Repositories;
using InfestCard.Models;
using InfestCard.Services.Interfaces;

namespace InfestCard.Services.Implementations;

public sealed class CardStorageService(ICardRepository cardRepository) : ICardStorageService
{
    public async Task<IReadOnlyList<Card>> GetCardsAsync(CancellationToken cancellationToken = default)
    {
        var entities = await cardRepository.GetAllAsync(cancellationToken);
        return entities.Select(MapToModel).ToList();
    }

    public async Task<Card?> GetCardByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var entity = await cardRepository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : MapToModel(entity);
    }

    public Task SaveCardAsync(Card card, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(card);
        ArgumentException.ThrowIfNullOrWhiteSpace(card.Id);

        if (card.Quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(card), "La cantidad de la carta no puede ser negativa.");
        }

        return cardRepository.UpsertAsync(MapToEntity(card), cancellationToken);
    }
    public Task DeleteCardAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        return cardRepository.DeleteAsync(id, cancellationToken);
    }

    private static Card MapToModel(CardEntity entity) => new()
    {
        Id = entity.Id,
        Type = entity.Type,
        Name = entity.Name,
        Description = entity.Description,
        Category = entity.Category,
        Level = entity.Level,
        //Image = entity.Image,
        ImageUrl = entity.ImageUrl,
        CreatedAt = entity.CreatedAt,
        LocalImagePath = entity.LocalImagePath,
        Quantity = entity.Quantity
    };

    private static CardEntity MapToEntity(Card card) => new()
    {
        Id = card.Id,
        Type = card.Type,
        Name = card.Name,
        Description = card.Description,
        Category = card.Category,
        Level = card.Level,
        //Image = card.Image,
        ImageUrl = card.ImageUrl,
        CreatedAt = card.CreatedAt,
        LocalImagePath = card.LocalImagePath,
        Quantity = card.Quantity
    };
}
