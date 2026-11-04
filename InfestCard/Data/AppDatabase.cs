using SQLite;
using InfestCard.Data.Entities;

namespace InfestCard.Data;

public sealed class AppDatabase
{
    private readonly SQLiteAsyncConnection connection;
    private readonly SemaphoreSlim initializationLock = new(1, 1);
    private bool isInitialized;

    public AppDatabase()
    {
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "infestcard.db3");
        connection = new SQLiteAsyncConnection(databasePath);
    }

    public async Task<SQLiteAsyncConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (isInitialized)
        {
            return connection;
        }

        await initializationLock.WaitAsync(cancellationToken);
        try
        {
            if (!isInitialized)
            {
                await connection.CreateTableAsync<CardEntity>();
                isInitialized = true;
            }
        }
        finally
        {
            initializationLock.Release();
        }

        return connection;
    }
}
