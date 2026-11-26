using InfestCard.ViewModels.Events;

namespace InfestCard.Views.Events;

public partial class InformationPage : ContentPage
{
    public InformationPage(InformationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}