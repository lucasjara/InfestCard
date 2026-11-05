using InfestCard.Data.Entities;

namespace InfestCard.Data.Repositories;

public sealed class CardRepository(AppDatabase database) : ICardRepository
{
    public async Task<IReadOnlyList<CardEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var connection = await database.GetConnectionAsync(cancellationToken);
        return await connection.Table<CardEntity>().ToListAsync();
    }

    public async Task<CardEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        var connection = await database.GetConnectionAsync(cancellationToken);
        return await connection.FindAsync<CardEntity>(id);
    }

    public async Task UpsertAsync(CardEntity card, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(card);
        ArgumentException.ThrowIfNullOrWhiteSpace(card.Id);
        cancellationToken.ThrowIfCancellationRequested();

        var connection = await database.GetConnectionAsync(cancellationToken);
        var existingCard = await connection.FindAsync<CardEntity>(card.Id);
        cancellationToken.ThrowIfCancellationRequested();

        if (existingCard is null)
        {
            await connection.InsertAsync(card);
            return;
        }

        await connection.UpdateAsync(card);
    }
}
