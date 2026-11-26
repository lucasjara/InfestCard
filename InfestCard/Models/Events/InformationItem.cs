namespace InfestCard.Models.Events;

public class InformationItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Edition { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string VenueName { get; set; } = string.Empty;
    public string VenueAddress { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string CoverImage { get; set; } = string.Empty;
    public string MapImage { get; set; } = string.Empty;

    public string EditionDisplay =>
        string.IsNullOrWhiteSpace(Edition) ? string.Empty : $"Edición {Edition}";

    public string DateRange
    {
        get
        {
            if (StartDate is null && EndDate is null)
                return "Fechas por confirmar";

            if (StartDate is not null && EndDate is not null)
                return $"{StartDate:dd MMM} - {EndDate:dd MMM yyyy}";

            var date = StartDate ?? EndDate;
            return date?.ToString("dd MMM yyyy") ?? string.Empty;
        }
    }

    public bool HasCoordinates => Latitude is not null && Longitude is not null;

    public string CoordinatesDisplay => HasCoordinates
        ? $"{Latitude:0.0000}, {Longitude:0.0000}"
        : "No disponible";
}