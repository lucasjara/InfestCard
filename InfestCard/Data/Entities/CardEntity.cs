using SQLite;

namespace InfestCard.Data.Entities;

[Table("Cards")]
public sealed class CardEntity
{
    [PrimaryKey]
    [MaxLength(200)]
    public string Id { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Type { get; set; } = string.Empty;

    [NotNull]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    public int Level { get; set; }

    public string? Image { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? LocalImagePath { get; set; }

    public int Quantity { get; set; }
}
