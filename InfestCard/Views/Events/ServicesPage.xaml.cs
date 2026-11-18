using InfestCard.ViewModels.Events;

namespace InfestCard.Views.Events;

public partial class ServicesPage : ContentPage
{
    public ServicesPage(ServicesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}