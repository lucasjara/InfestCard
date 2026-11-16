using InfestCard.ViewModels.Events;

namespace InfestCard.Views.Events;

public partial class ExplorerMapPage : ContentPage
{
    public ExplorerMapPage(ExplorerMapViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
