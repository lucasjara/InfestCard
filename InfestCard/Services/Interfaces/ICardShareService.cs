namespace InfestCard.Services.Interfaces;

public interface ICardShareService
{
    Task ShareAsync(byte[] imageBytes,string title,CancellationToken cancellationToken = default);
}