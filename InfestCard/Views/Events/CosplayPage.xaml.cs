using InfestCard.ViewModels.Events;

namespace InfestCard.Views.Events;

public partial class CosplayPage : ContentPage
{
    public CosplayPage(CosplayViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}