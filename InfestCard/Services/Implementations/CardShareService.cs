using InfestCard.Services.Interfaces;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace InfestCard.Services.Implementations;

public sealed class CardShareService : ICardShareService
{
    public async Task ShareAsync(
        byte[] imageBytes,
        string title,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(imageBytes);

        if (imageBytes.Length == 0)
            throw new ArgumentException("La imagen está vacía.");

        cancellationToken.ThrowIfCancellationRequested();

        var filePath = Path.Combine(
            FileSystem.CacheDirectory,
            $"card-{Guid.NewGuid():N}.png");

        await File.WriteAllBytesAsync(
            filePath,
            imageBytes,
            cancellationToken);

        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = title,
            File = new ShareFile(filePath, "image/png")
        });
    }
}