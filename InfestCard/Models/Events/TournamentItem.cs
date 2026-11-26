namespace InfestCard.Models.Events;

public class TournamentItem
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Game { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Format { get; set; } = string.Empty;

    public string Status { get; set; } = "upcoming";

    public int? ParticipantsCount { get; set; }

    public string Location { get; set; } = string.Empty;

    public string Image { get; set; } = "event_guide_background.png";

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public string TimeRange => $"{StartAt:HH\\:mm} - {EndAt:HH\\:mm}";

    public string ParticipantsText => ParticipantsCount.HasValue ? $"Participantes: {ParticipantsCount}" : string.Empty;

    public bool HasParticipants => ParticipantsCount.HasValue;

    public bool HasGame => !string.IsNullOrWhiteSpace(Game);

    public bool HasFormat => !string.IsNullOrWhiteSpace(Format);
}