using InfestCard.ViewModels;
using InfestCard.Models;

namespace InfestCard.Views;

public partial class CollectionPage : ContentPage
{
    public CollectionPage(CollectionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.RefreshCommand.ExecuteAsync(null);
    }

    private CollectionViewModel ViewModel => (CollectionViewModel)BindingContext;

    private async void OnCardSelected(object? sender, SelectionChangedEventArgs eventArgs)
    {
        if (eventArgs.CurrentSelection.FirstOrDefault() is not Card card)
        {
            return;
        }

        CardsView.SelectedItem = null;
        await ViewModel.SelectCardCommand.ExecuteAsync(card);
    }
}
