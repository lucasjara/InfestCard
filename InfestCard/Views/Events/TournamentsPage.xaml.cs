using InfestCard.ViewModels.Events;

namespace InfestCard.Views.Events;

public partial class TournamentsPage : ContentPage
{
    public TournamentsPage(TournamentsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}