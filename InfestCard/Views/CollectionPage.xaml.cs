using InfestCard.ViewModels;

namespace InfestCard.Views;

public partial class CollectionPage : ContentPage
{
    public CollectionPage(CollectionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
