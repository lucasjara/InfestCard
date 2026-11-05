using System.Text.Json;
using InfestCard.Configuration;
using InfestCard.Models;
using InfestCard.Services.Interfaces;

namespace InfestCard.Services.Implementations;

public sealed class CardApiService(ApiOptions options) : ICardApiService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<Card> GetCardFromQrAsync(
        string qrPayload,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(qrPayload);
        cancellationToken.ThrowIfCancellationRequested();

        CardApiResponse? response;
        try
        {
            response = JsonSerializer.Deserialize<CardApiResponse>(qrPayload, SerializerOptions);
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException("El contenido del QR no tiene un formato JSON válido.", exception);
        }

        if (response?.Card is null || string.IsNullOrWhiteSpace(response.Card.Id))
        {
            throw new InvalidDataException("El JSON del QR no contiene una carta válida.");
        }

        await Task.CompletedTask;
        return MapCard(response.Card, response.Quantity);
    }

    private Card MapCard(Card card, int quantity)
    {
        return new Card
        {
            Id = card.Id,
            Type = card.Type,
            Name = card.Name,
            Description = card.Description,
            Category = card.Category,
            Level = card.Level,
            Image = card.Image,
            ImageUrl = ResolveImageUrl(card.ImageUrl),
            CreatedAt = card.CreatedAt,
            LocalImagePath = card.LocalImagePath,
            Quantity = quantity
        };
    }

    private string? ResolveImageUrl(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl) || Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
        {
            return imageUrl;
        }

        if (string.IsNullOrWhiteSpace(options.BaseUrl) || !Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out var baseUri))
        {
            return imageUrl;
        }

        return new Uri(baseUri, imageUrl).ToString();
    }
}
