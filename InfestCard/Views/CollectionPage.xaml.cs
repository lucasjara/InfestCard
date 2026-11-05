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
        try
        {
            await ViewModel.RefreshCommand.ExecuteAsync(null);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[INFESTCard][CollectionPage] Error al refrescar la colección: {ex.Message}");
            await DisplayAlert("Error", "No se pudo actualizar la colección. Por favor, inténtalo de nuevo más tarde.", "OK");
        }
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
