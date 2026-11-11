using System.Runtime.CompilerServices;
using InfestCard.ViewModels;
using InfestCard.Models;

namespace InfestCard.Views;

public partial class CollectionPage : ContentPage
{
    private readonly ConditionalWeakTable<Border, object> animatedCards = new();

    private int cardAnimationIndex;
    private bool hasPlayedEntrance;

    public CollectionPage(CollectionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private CollectionViewModel ViewModel =>
        (CollectionViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            if (!hasPlayedEntrance)
            {
                hasPlayedEntrance = true;
                await AnimatePageEntranceAsync();
            }

            await ViewModel.RefreshCommand.ExecuteAsync(null);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"[INFESTCard][CollectionPage] Error al refrescar la colección: {ex.Message}");

            await DisplayAlert(
                "Error",
                "No se pudo actualizar la colección. Por favor, inténtalo de nuevo más tarde.",
                "OK");
        }
    }

    // Animación de entrada de la pantalla
    private async Task AnimatePageEntranceAsync()
    {
        var headerAnimation = Task.WhenAll(
            HeaderView.FadeTo(1, 300, Easing.CubicOut),
            HeaderView.TranslateTo(0, 0, 300, Easing.CubicOut)
        );

        var filtersAnimation = Task.WhenAll(
            FiltersView.FadeTo(1, 280, Easing.CubicOut),
            FiltersView.TranslateTo(0, 0, 280, Easing.CubicOut)
        );

        var scannerAnimation = AnimateScannerButtonAsync();

        await Task.WhenAll(
            headerAnimation,
            filtersAnimation,
            scannerAnimation
        );
    }

    // Animación del botón QR
    private async Task AnimateScannerButtonAsync()
    {
        await Task.Delay(100);

        await ScannerButton.ScaleTo(
            1,
            280,
            Easing.SpringOut
        );
    }

    // Animación escalonada de las cartas
    private async void OnCardLoaded(object? sender, EventArgs e)
    {
        if (sender is not Border cardView)
            return;

        // Evita repetir la animación sobre una vista reciclada
        if (animatedCards.TryGetValue(cardView, out _))
            return;

        animatedCards.Add(cardView, new object());

        // Separación entre las animaciones
        int animationOrder = cardAnimationIndex++ % 6;

        await Task.Delay(animationOrder * 45);

        await Task.WhenAll(
            cardView.FadeTo(1, 250, Easing.CubicOut),
            cardView.TranslateTo(0, 0, 250, Easing.CubicOut)
        );
    }

    private async void OnCardSelected(
        object? sender,
        SelectionChangedEventArgs eventArgs)
    {
        if (eventArgs.CurrentSelection.FirstOrDefault() is not Card card)
            return;

        CardsView.SelectedItem = null;

        await ViewModel.SelectCardCommand.ExecuteAsync(card);
    }
}