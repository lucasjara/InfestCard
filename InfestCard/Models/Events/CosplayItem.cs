namespace InfestCard.Models.Events;

public class CosplayItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Rules { get; set; } = string.Empty;

    public string Participants { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string Image { get; set; } = "event_guide_background.png";

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public string Status { get; set; } = "scheduled";

    public string TimeRange => $"{StartAt:HH\\:mm} - {EndAt:HH\\:mm}";

    public bool HasRules => !string.IsNullOrWhiteSpace(Rules);

    public bool HasParticipants => !string.IsNullOrWhiteSpace(Participants);
}