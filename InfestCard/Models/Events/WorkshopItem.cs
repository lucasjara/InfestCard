namespace InfestCard.Models.Events;

public class WorkshopItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Instructor { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Requirements { get; set; } = string.Empty;

    public int? Capacity { get; set; }

    public string Location { get; set; } = string.Empty;

    public string Icon { get; set; } = "🛠️";

    public string Image { get; set; } = "event_guide_background.png";

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public string Status { get; set; } = "scheduled";

    public string TimeRange => $"{StartAt:HH\\:mm} - {EndAt:HH\\:mm}";

    public string CapacityText => Capacity.HasValue ? $"Cupos: {Capacity}" : string.Empty;

    public bool HasRequirements => !string.IsNullOrWhiteSpace(Requirements);
}