namespace InfestCard.Models;

public sealed class Card
{
    public string Id { get; init; } = string.Empty;

    public string Type { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string Category { get; init; } = string.Empty;

    public int Level { get; init; }

    public string? Image { get; init; }

    public string? ImageUrl { get; init; }

    public DateTime CreatedAt { get; init; }

    public string? LocalImagePath { get; init; }

    public int Quantity { get; init; }
}
