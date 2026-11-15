
using InfestCard.ViewModels.Events;

namespace InfestCard.Views.Events;

public partial class StandsPage : ContentPage
{
    public StandsPage(StandsViewModel viewModel)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = viewModel;
    }
}
