namespace InfestCard.Models.Events;

public class ActivityItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string StageName { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string Icon { get; set; } = "🎭";

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public string Status { get; set; } = "scheduled";

    public string TimeRange => $"{StartAt:HH\\:mm} - {EndAt:HH\\:mm}";
}