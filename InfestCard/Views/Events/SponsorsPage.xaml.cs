using InfestCard.ViewModels.Events;

namespace InfestCard.Views.Events;

public partial class SponsorsPage : ContentPage
{
    public SponsorsPage(SponsorsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}