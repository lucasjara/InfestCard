using InfestCard.Models;
using InfestCard.Services.Interfaces;

namespace InfestCard.Services.Implementations;

public sealed class CardImageService(HttpClient httpClient) : ICardImageService
{
    private const string PlaceholderResource = "card_placeholder.svg";

    public async Task<Card> PrepareImageAsync(
        Card card,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(card);
        cancellationToken.ThrowIfCancellationRequested();

        if (IsUsableLocalImage(card.LocalImagePath))
        {
            return card;
        }

        if (Uri.TryCreate(card.ImageUrl, UriKind.Absolute, out var imageUri) &&
            (imageUri.Scheme == Uri.UriSchemeHttp || imageUri.Scheme == Uri.UriSchemeHttps))
        {
            var localImagePath = await DownloadIfNeededAsync(card.Id, imageUri, cancellationToken);
            if (localImagePath is not null)
            {
                return CopyWithLocalImagePath(card, localImagePath);
            }
        }

        return CopyWithLocalImagePath(card, GetPlaceholderResource());
    }

    private async Task<string?> DownloadIfNeededAsync(
        string cardId,
        Uri imageUri,
        CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(imageUri.AbsolutePath);
        if (string.IsNullOrWhiteSpace(extension) || extension.Length > 10)
        {
            extension = ".img";
        }

        var fileName = $"card_{Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes(cardId + imageUri.AbsoluteUri)))[..16]}{extension}";
        var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        if (File.Exists(filePath) && new FileInfo(filePath).Length > 0)
        {
            return filePath;
        }

        try
        {
            using var response = await httpClient.GetAsync(imageUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();
            await using var source = await response.Content.ReadAsStreamAsync(cancellationToken);
            await using var destination = File.Create(filePath);
            await source.CopyToAsync(destination, cancellationToken);
            return new FileInfo(filePath).Length > 0 ? filePath : null;
        }
        catch (HttpRequestException exception)
        {
            System.Diagnostics.Debug.WriteLine($"[INFESTCard][Image] Descarga fallida: {exception.Message}");
            TryDelete(filePath);
            return null;
        }
        catch (IOException exception)
        {
            System.Diagnostics.Debug.WriteLine($"[INFESTCard][Image] Guardado fallido: {exception.Message}");
            TryDelete(filePath);
            return null;
        }
    }

    private static string GetPlaceholderResource() => PlaceholderResource;

    private static bool IsUsableLocalImage(string? path) =>
        !string.IsNullOrWhiteSpace(path) && File.Exists(path) && new FileInfo(path).Length > 0;

    private static Card CopyWithLocalImagePath(Card card, string localImagePath) => new()
    {
        Id = card.Id,
        Type = card.Type,
        Name = card.Name,
        Description = card.Description,
        Category = card.Category,
        Level = card.Level,
        Image = card.Image,
        ImageUrl = card.ImageUrl,
        CreatedAt = card.CreatedAt,
        LocalImagePath = localImagePath,
        Quantity = card.Quantity
    };

    private static void TryDelete(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch (IOException)
        {
        }
    }
}
