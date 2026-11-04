using InfestCard.Data.Entities;

namespace InfestCard.Data.Repositories;

public interface ICardRepository
{
    Task<IReadOnlyList<CardEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<CardEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task UpsertAsync(CardEntity card, CancellationToken cancellationToken = default);
}
