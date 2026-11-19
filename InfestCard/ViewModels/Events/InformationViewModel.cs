using InfestCard.Models.Events;
using CommunityToolkit.Mvvm.ComponentModel;

namespace InfestCard.ViewModels.Events;

public partial class InformationViewModel : ObservableObject
{
    [ObservableProperty]
    private InformationItem information = new();

    public InformationViewModel()
    {
        LoadDefaultInformation();
    }

    private void LoadDefaultInformation()
    {
        var startDate = DateTime.Today.AddDays(1);

        Information = new InformationItem
        {
            Id = 1,
            Name = "INFEST Chile",
            Edition = "2026",
            Description = "El evento de cultura pop, gaming y tecnología más grande de Chile. " +
                          "Tres días de stands, torneos, cosplay, talleres y experiencias para toda la comunidad.",
            StartDate = startDate,
            EndDate = startDate.AddDays(2),
            VenueName = "Centro de Eventos INFEST",
            VenueAddress = "Av. Principal 1234, Santiago, Chile",
            Latitude = null,
            Longitude = null,
            CoverImage = "event_guide_background2.png",
            MapImage = string.Empty
        };
    }
}